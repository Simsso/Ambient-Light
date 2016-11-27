#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

byte r, g, b, // target
  curR, curG, curB, // current
  lastR, lastG, lastB; // last 
const byte outputPin = 8,
  numLEDs = 12;

float lastFac = 0, targetFac = 0;

const int transitionTimeMillis = 100;

unsigned long lastColorDataReceived = 0, millisIntoTransition = 0;

Adafruit_NeoPixel light = Adafruit_NeoPixel(
  numLEDs, 
  outputPin, 
  NEO_GRB + NEO_KHZ800);

void setup() {
  Serial.begin(9600);
  
  light.begin(); // initialize NeoPixel library
  
  showInitTest();
}

void loop() {
  if (Serial.available() == 3) {
    lastR = curR; lastG = curG; lastB = curB;
    r = Serial.read(); g = Serial.read(); b = Serial.read();
    lastColorDataReceived = millis();
  }
  if (Serial.available() > 3) {
    Serial.flush();
  }
  millisIntoTransition = millis() - lastColorDataReceived;
  Serial.println(millisIntoTransition);
  if (millisIntoTransition >= transitionTimeMillis) {
    curR = r; curG = g; curB = b;
  }
  else {
    lastFac = (float)(transitionTimeMillis - millisIntoTransition) / (float)transitionTimeMillis;
    targetFac = 1 - lastFac;
    curR = lastR * lastFac + r * targetFac;
    curG = lastG * lastFac + g * targetFac;
    curB = lastB * lastFac + b * targetFac;
  }
  setOutputColor(curR, curG, curB);
}

void setOutputColor(byte r, byte g, byte b) {
  for (int i = 0; i < numLEDs; i++) {
    light.setPixelColor(i, light.Color(r, g, b));
  }
  light.show();
}

void showInitTest() {
  setOutputColor(255, 0, 0);
  delay(1000);
  setOutputColor(0, 255, 0);
  delay(1000);
  setOutputColor(0, 0, 255);
  delay(1000);
  setOutputColor(0, 0, 0);
}
