void setup() {
  // put your setup code here, to run once:
Serial.begin(9600);
}
int x,y,z;
void loop() {
  // put your main code here, to run repeatedly:
x=analogRead(A0);
y=analogRead(A1);
z=analogRead(A2);
Serial.print(x);
Serial.print(":");
Serial.print(y);
Serial.print(":");
Serial.print(z);
Serial.println();
delay(100);
}
