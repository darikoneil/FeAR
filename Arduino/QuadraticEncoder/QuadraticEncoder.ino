
// This is a quadratic encoder

//This reads quadratic encoder data and then sends translated data to a unity messaging listener

//declarations
int pinA = 11; //Serial Pin A
int pinB = 10; // Serial Pin B
int pulses = 0; // Pulses
int prevPin = LOW; //Pin A Previous State
int currPin; //Pin A Current State
int unityCMD; //Unity Command

void setup() {
  //Setup Pins & Establish Serial Connection
  pinMode(pinA,INPUT);
  pinMode(pinB,INPUT);
  Serial.begin(57600);
}

void loop() {
  //Read Current Pin State
  currPin = digitalRead(pinA);
  
  // if A then B Clockwise
  //if B then A Counter-Clockwise
  if((prevPin == LOW) && (currPin == HIGH)) {
    if(digitalRead(pinB) == LOW) {
      pulses--; //-1
    }
    else {
      pulses++; //+1
    }
  }

  //Check for Unity command
  if(Serial.available()>0) { 
    unityCMD = Serial.read();

    // Print accumulated pulses
    Serial.println(pulses);
    
   // Reset pulses
   pulses = 0;
  }

  //set prevPin
  prevPin = currPin;
}
