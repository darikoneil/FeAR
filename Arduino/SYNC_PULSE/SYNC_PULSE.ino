
//This Sends a sync-pulse to the imaging stream to unify the behavioral data & imaging data

//Voff = 0.56V
//Vref = 2.2V
// 0<=Xin<=4095
//Vout = Vref*(Xin/[(2^N)-1])+Voffset
//N = 2^12 given 12bit
int state = 0;
int unityCMD;

void setup() {
Serial.begin(57600);
analogWriteResolution(12);
}

void loop() {
  //Check for Unity command
  if(Serial.available()>0) {
    unityCMD = Serial.read();

    //If command change the voltage output
    if(state == 4095) {
      analogWrite(DAC0,0); //DOWN STATE
      state = 0;
    }
    else
    {
      analogWrite(DAC0,4095); // UP STATE
      state = 4095;
    }
  }
}
