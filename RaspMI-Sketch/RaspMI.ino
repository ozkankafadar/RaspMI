#define BUZZER  9//Buzzer Pin
#define SELPIN 10 //Selection Pin
#define DATAOUT 11//MISO Pin
#define DATAIN  12//MOSI Pin
#define SPICLOCK  13//Clock Pin

boolean startStop = false;
int V,NS,EW;
String sentData;
char receivedData='-';

void setup(){
//Set pin modes
  pinMode(SELPIN, OUTPUT);
  pinMode(DATAOUT, OUTPUT);
  pinMode(DATAIN, INPUT);
  pinMode(SPICLOCK, OUTPUT);
  pinMode(BUZZER, OUTPUT);
//Disable device to start with
  digitalWrite(SELPIN,HIGH);
  digitalWrite(DATAOUT,LOW);
  digitalWrite(SPICLOCK,LOW);
  digitalWrite(BUZZER,LOW);
//Set baudrate
  Serial.begin(115200);
//Stop interrupts
  cli();
//Set entire TCCR1A register to 0
  TCCR1A = 0;
//Set entire TCCR1B register to 0
  TCCR1B = 0;
//Set counter value to 0
  TCNT1  = 0;
//Set compare match register for 100 Hz increments
//OCR1A = clockSpeed/(frequency*preScaler)-1;
  OCR1A = 19999;
//Turn on CTC mode
  TCCR1B |= (1 << WGM12);
//Set CS11 bits for 64 prescaler
  TCCR1B |= (1 << CS11);
//Enable timer compare interrupt
  TIMSK1 |= (1 << OCIE1A);
//Start interrupts
  sei();  
}

 int read_ADC(int ID){
 int adcValue = 0;
 byte commandbits = B11000000;
 commandbits|=((ID-1)<<3);
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
      adcValue+=digitalRead(DATAIN)<<i;
      digitalWrite(SPICLOCK,HIGH);
      digitalWrite(SPICLOCK,LOW);
    }    
 digitalWrite(SELPIN, HIGH);
 return adcValue;
}

ISR(TIMER1_COMPA_vect) {
  if (startStop)
  {
    V = read_ADC(1);
    NS = read_ADC(2);
    EW = read_ADC(3);    
    sentData = String(V) + "*" + String(NS) + "*" + String(EW);
    Serial.println(sentData);
  }
}

void loop() {  
  if (Serial.available()) {
    receivedData = Serial.read();
    if(receivedData=='1')
    {
      startStop=true;
    }
    else if(receivedData=='2')
    {
      startStop=false;
      for(int i=0;i<3;i++)
      {
        digitalWrite(BUZZER,HIGH);
        delay(500);
        digitalWrite(BUZZER,LOW);
        delay(500);      
      }      
    }
    else if(receivedData=='3')
    {
      digitalWrite(BUZZER,HIGH);
      delay(500);
      digitalWrite(BUZZER,LOW);
      delay(500);
    }
  }  
}
