const int pwmPinR = 3, pwmPinG = 5, pwmPinB = 6;

byte r, g, b;

void setup() {
  pinMode(pwmPinR, OUTPUT);
  pinMode(pwmPinG, OUTPUT);
  pinMode(pwmPinB, OUTPUT);

  Serial.begin(9600);
}

void loop() {
  if (Serial.available() == 3) {
    r = Serial.read();
    g = Serial.read();
    b = Serial.read();

    analogWrite(pwmPinR, r);
    analogWrite(pwmPinG, g);
    analogWrite(pwmPinB, b);
  }
}
