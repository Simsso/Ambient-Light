#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

byte r, g, b;
const byte outputPin = 8,
  numLEDs = 12;

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
    r = Serial.read();
    g = Serial.read();
    b = Serial.read();

    setOutputColor(r, g, b);
  }
  if (Serial.available() > 3) {
    Serial.flush();
  }
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
