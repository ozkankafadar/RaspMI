#define SELPIN 10 //Selection Pin
#define DATAOUT 11//MISO Pin
#define DATAIN  12//MOSI Pin
#define SPICLOCK  13//Clock Pin
#define BUZZER  9//Buzzer Pin

boolean startstop = false;
int pos[3];
int channels[3] = {1, 2, 3};
String str;

void setup(){
//set pin modes
  pinMode(SELPIN, OUTPUT);
  pinMode(DATAOUT, OUTPUT);
  pinMode(DATAIN, INPUT);
  pinMode(SPICLOCK, OUTPUT);
  pinMode(BUZZER, OUTPUT);

//disable device to start with
  digitalWrite(SELPIN,HIGH);
  digitalWrite(DATAOUT,LOW);
  digitalWrite(SPICLOCK,LOW);
  digitalWrite(BUZZER,LOW);

  Serial.begin(115200);//Sets the data rate in bits per second (baud) for serial data transmission
  
  cli();//stop interrupts
  TCCR1A = 0;// set entire TCCR1A register to 0
  TCCR1B = 0;// set entire TCCR1B register to 0
  TCNT1  = 0;//initialize counter value to 0
  
  // set compare match register for 1hz increments
  //200 Hz:9999;100 Hz:19999
  OCR1A = 19999;//OCR1A = clockSpeed/(frequency*preScaler)-1;
  TCCR1B |= (1 << WGM12);// turn on CTC mode
  TCCR1B |= (1 << CS11);// Set CS11 bits for 64 prescaler
  TIMSK1 |= (1 << OCIE1A);// enable timer compare interrupt
  sei();//start interrupts  
}

 int read_adc(int channel){
 int adcvalue = 0;
 byte commandbits = B11000000;
 commandbits|=((channel-1)<<3);
 digitalWrite(SELPIN,LOW);
    for (int i=7; i>=3; i--){
      digitalWrite(DATAOUT,commandbits&1<<i);
      digitalWrite(SPICLOCK,HIGH);
      digitalWrite(SPICLOCK,LOW);    
    }
 digitalWrite(SPICLOCK,HIGH);
 digitalWrite(SPICLOCK,LOW);
 digitalWrite(SPICLOCK,HIGH);  
 digitalWrite(SPICLOCK,LOW);

    for (int i=11; i>=0; i--){
      adcvalue+=digitalRead(DATAIN)<<i;
      digitalWrite(SPICLOCK,HIGH);
      digitalWrite(SPICLOCK,LOW);
    }
    
 digitalWrite(SELPIN, HIGH);
 return adcvalue;
}

ISR(TIMER1_COMPA_vect) {
  if (startstop)
  {
    pos[0] = read_adc(1);
    pos[1] = read_adc(2);
    pos[2] = read_adc(3); 
    
    str = String(pos[0]) + "*" + String(pos[1]) + "*" + String(pos[2]);
    Serial.println(str);
  }
}

char incomingByte='-';

void loop() {
  
  if (Serial.available()) {
    incomingByte = Serial.read();    

    if(incomingByte=='1')
    {
      startstop=true;
    }
    else if(incomingByte=='2')
    {
      startstop=false;
      digitalWrite(BUZZER,HIGH);delay(500);
      digitalWrite(BUZZER,LOW);delay(500);
      digitalWrite(BUZZER,HIGH);delay(500);
      digitalWrite(BUZZER,LOW);delay(500);
      digitalWrite(BUZZER,HIGH);delay(500);
      digitalWrite(BUZZER,LOW);delay(500);
    }
    else if(incomingByte=='3')
    {
      digitalWrite(BUZZER,HIGH);delay(500);
      digitalWrite(BUZZER,LOW);delay(500);
      digitalWrite(BUZZER,HIGH);delay(500);
      digitalWrite(BUZZER,LOW);delay(500);
      digitalWrite(BUZZER,HIGH);delay(500);
      digitalWrite(BUZZER,LOW);delay(500);
    }
  }  
} 
