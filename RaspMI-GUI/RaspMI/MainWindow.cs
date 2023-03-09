using System;
using Gtk;
using System.IO;
using Cairo;
using System.Collections;
using System.Timers;
using System.Linq;

public partial class MainWindow: Gtk.Window
{
	System.IO.Ports.SerialPort deviceSp,gpsSp;

	System.Threading.Thread recordingThread,graphicsThread,saveThread;

	Timer timer1,timer2;

	Cairo.Context g2;

	StreamWriter sw;

	int drw=0,drh=0,cnt=10,val=0;

	int[] datam=new int[100];

	ArrayList data_a=new ArrayList();
	ArrayList data_b=new ArrayList();
	ArrayList data_c=new ArrayList();
	ArrayList data3a=new ArrayList();
	ArrayList data3b=new ArrayList();
	ArrayList data3c=new ArrayList();

	long dc=0,count=0,timercount=0;

	string fileName,Ttime,Tlat,Tlon,Tdate,GPStxt,data;
	string[] data2;

	DateTime tar ;

	bool cont = false,save = false, rec=false;//conv=false;

	PointD p1,p2,p1b,p2b,p1c,p2c,po1a,po1b,po2a,po2b,po3a,po3b;

	float vv=0,v=0;

	public uint timerID;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		this.timeinfodrwarea.ModifyBg (StateType.Normal,new Gdk.Color(255,0,0));
		this.plotdrwarea.ModifyBg (StateType.Normal,new Gdk.Color(0,0,0));

		timecombo.Active = 0;
		connectbtn.Sensitive = true;
		startbtn.Sensitive = false;
		stopbtn.Sensitive = false;
		devicelbl.Text="Device : Not Ready";
		GPSlbl.Text="GPS : Not Ready";
		GPStxt = null;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		if (deviceSp.IsOpen) {

				cont = false;

				if (recordingThread.IsAlive) {
					recordingThread.Abort ();
				}		
		}

		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnConnectbtnClicked (object sender, EventArgs e)
	{
		if (connectbtn.Label == "CONNECT") {
			try {
				string portname = "/dev/ttyACM0";
				deviceSp = new System.IO.Ports.SerialPort ();
				deviceSp.PortName = portname;
				deviceSp.BaudRate = 115200;
				deviceSp.Open ();

				System.Threading.Thread.Sleep(2000);

				devicelbl.Text = "Device : Ready";
				connectbtn.Label = "DISCONNECT";
				startbtn.Sensitive = true;
				stopbtn.Sensitive = false;

				g2 = Gdk.CairoHelper.Create (timeinfodrwarea.GdkWindow);
				g2.SetFontSize (16);

				deviceSp.Write ("1");
				deviceSp.DiscardInBuffer ();

				drw = plotdrwarea.Allocation.Width;
				drh = plotdrwarea.Allocation.Height;

				v=0;
				vv = (float)(drw / 1000.0);
				cont = true;

				plotdrwarea.GdkWindow.Clear ();

				recordingThread = new System.Threading.Thread (new System.Threading.ThreadStart (Run));

				if (!recordingThread.IsAlive)
					recordingThread.Start ();
				System.Threading.Thread.Sleep(100);

				save = true;
			} catch {
				devicelbl.Text = "Device : Not Ready";
			}
		}
		else{
			if (deviceSp.IsOpen) {
				cont = false;

				if (recordingThread.IsAlive) {
					recordingThread.Abort();
				}

				deviceSp.Write("2");
				deviceSp.Close ();

				System.Threading.Thread.Sleep (1000);
				devicelbl.Text="Device : Not Ready";
				connectbtn.Label="CONNECT";
				timeinfodrwarea.GdkWindow.Clear ();
				this.timeinfodrwarea.ModifyBg (StateType.Normal,new Gdk.Color(255,0,0));
				stopbtn.Sensitive = false;
				System.Threading.Thread.Sleep (10);
				startbtn.Sensitive = false;
				System.Threading.Thread.Sleep (10);			

				GLib.Source.Remove (timerID);
				save = false;
				data3a.Clear ();
				data3b.Clear ();
				data3c.Clear ();
				data_a.Clear ();
				data_b.Clear ();
				data_c.Clear ();
			}
		}
	}

	bool pause=false;

		public void Run ()
		{
			data3a.Clear ();
			data3b.Clear ();
			data3c.Clear ();

			for (int k = 0; k < 10; k++)
				data = deviceSp.ReadLine ();

		while (true) {		
			try {
				data = deviceSp.ReadLine ();
			} catch {
			}
			if (data != null)
				data.Replace ("\r", "").Replace ("?", "").Trim ();

			if (rec) {
				if (sw != null)
					sw.WriteLine (data);
				count++;

				if (count >= dc) {
					pause = true;
					timer1.Stop ();
					sw.Close ();
					sw = null;
					rec = false;							
					data3a.Clear ();
					data3b.Clear ();
					data3c.Clear ();

					Gtk.Application.Invoke (delegate {
						startbtn.Sensitive = true;							
						stopbtn.Sensitive = false;							
						connectbtn.Sensitive = true;							
					});

					Gtk.Application.Invoke (delegate {
						timeinfodrwarea.GdkWindow.Clear ();
						this.timeinfodrwarea.ModifyBg (StateType.Normal, new Gdk.Color (255, 0, 0));
					});

					Gtk.Application.Invoke (delegate {
						System.Threading.Tasks.Parallel.Invoke (SaveData);
					});

					deviceSp.Write ("3");

					Gtk.Application.Invoke (delegate {
						devicelbl.Text = "Device : Ready";
						timeinfodrwarea.GdkWindow.Clear ();
						this.timeinfodrwarea.ModifyBg (StateType.Normal, new Gdk.Color (255, 0, 0));
					});
		
					pause = false;
				}
			}

			if (save) {
				if (data != "") {
					data2 = data.Split ('*');						
					bool sonuc;
					if (data2.Length > 0) {
						sonuc = int.TryParse (data2 [0], out val);
						if (sonuc) {
							data3a.Add (val);
						}
					} else {
						data3a.Add (0);
					}

					if (data2.Length > 1) {
						sonuc = int.TryParse (data2 [1], out val);
						if (sonuc) {
							data3b.Add (val);
						}
					} else {
						data3b.Add (0);
					}

					if (data2.Length > 2) {
						sonuc = int.TryParse (data2 [2], out val);
						if (sonuc) {
							data3c.Add (val);
						}
					} else {
						data3c.Add (0);
					}
				}

				if (data3a.Count == 100) {
					Gtk.Application.Invoke (delegate {
						System.Threading.Tasks.Parallel.Invoke (Update);
					});
				}
			}
		}
	}

	public void SaveData()
	{
			string path = "Data/" + fileName + ".txt";
			StreamReader sr = new StreamReader (path);
			string[] rows = System.IO.File.ReadAllLines (path);
			sr.Close ();

			double[] dataV = new double[rows.Length];
			double[] dataNS = new double[rows.Length];
			double[] dataEW = new double[rows.Length];

			for (int i=0; i<rows.Length; i++) {
				string[] rowstr = rows [i].Split ('*');
				int[] row = {int.Parse(rowstr[0]),int.Parse(rowstr[1]),int.Parse(rowstr[2])};
				dataV [i] = (double)row [0];
				dataNS [i] = (double)row [1];
				dataEW [i] = (double)row [2];
			}			

			double avgV = dataV.Sum () / rows.Count ();
			double avgNS = dataNS.Sum () / rows.Count ();
			double avgEW = dataEW.Sum () / rows.Count ();

			for (int i=0; i<rows.Length; i++) {
				dataV [i] -= avgV;
				dataNS [i] -= avgNS;
				dataEW [i] -= avgEW;
			}

		sacHeader headZ = new sacHeader (100f, fileName, dc * 10, tar, Tdate,Ttime, "TRZ"); 
		sacdel d1=new sacdel(headZ.createSACFile);
		d1.Invoke(headZ, dataV);

		sacHeader headN = new sacHeader (100f, fileName, dc * 10, tar, Tdate,Ttime, "TRN"); 
		sacdel d2=new sacdel(headN.createSACFile);
		d2.Invoke(headN, dataNS);

		sacHeader headE = new sacHeader (100f, fileName, dc * 10, tar, Tdate,Ttime, "TRE"); 
		sacdel d3=new sacdel(headE.createSACFile);
		d3.Invoke(headE, dataEW);
	}

	public delegate void sacdel (sacHeader sH, double[] data);

		public class sacHeader
		{
			public float sampleRate{ get; set; }
			public string filename{ get; set; }
			public string component{ get; set; }
			public long time{ get; set; }
			public DateTime Tar{ get; set; }
			public string Tdatem{ get; set; }
			public string Ttimem{ get; set; }
			public float[] array1{ get; set; }
			public float b{ get; set; }
			public float e{ get; set; }
			public float[] array2{ get; set; }
			public Int32[] array3{ get; set; }
			public Int32[] array4{ get; set; }
			public Int32 nvhdr{ get; set; }
			public Int32[] array5{ get; set; }
			public Int32 npts{ get; set; }
			public Int32[] array6{ get; set; }
			public int  iftype{ get; set; }
			public Int32[] array7{ get; set; }
			public char[] kstnmchar{ get; set; }
			public char[] kevnmchar{ get; set; }
			public char[] kholechar{ get; set; }
			public char[] kochar{ get; set; }
			public char[] kachar{ get; set; }
			public char[] ktchar{ get; set; }
			public char[] kfchar{ get; set; }
			public char[] kuserchar{ get; set; }
			public char[] kcmpnmchar{ get; set; }
			public char[] knetwkchar{ get; set; }
			public char[] kdatrdchar{ get; set; }
			public char[] kinstchar{ get; set; }

			public sacHeader (float samp, string file, long tim, DateTime tari, string tdatem, string ttime, string comp)
			{
				sampleRate = 1f / samp;
				filename = file;
				time = tim;
				Tar = tari;
				Tdatem = tdatem;
				Ttimem = ttime;
				component = comp;

				array1 = new float[4];
				for (int i=0; i<4; i++) {
					array1 [i] = float.Parse ("-12345");
				}
				b = float.Parse ("0");
				e = float.Parse ((time / 1000).ToString ());
				array2 = new float[63];
				for (int i=0; i<63; i++) {
					array2 [i] = float.Parse ("-12345");
				}

				array3 = new int[3];
				if (Tdatem != "" && Tdatem != null) {
					array3 [0] = Int32.Parse (Tdatem.Substring (4, 2) + 2000);
					array3 [1] = Int32.Parse (Tdatem.Substring (0, 2));
					array3 [2] = Int32.Parse (Ttimem.Substring (0, 2));
				} else {
					array3 [0] = Int32.Parse (Tar.Year.ToString ());
					array3 [1] = Int32.Parse (Tar.Day.ToString ());
					array3 [2] = Int32.Parse (Tar.Hour.ToString ());
				}

				array4 = new int[3];
				if (Tdatem != "" && Tdatem != null) {
					array4 [0] = Int32.Parse (Ttimem.Substring (2, 2));
					array4 [1] = Int32.Parse (Ttimem.Substring (4, 2));
					array4 [2] = Int32.Parse (Ttimem.Substring (4, 2));
				} else {
					array4 [0] = Int32.Parse (Tar.Minute.ToString ());
					array4 [1] = Int32.Parse (Tar.Second.ToString ());
					array4 [2] = Int32.Parse (Tar.Second.ToString ());
				}

				nvhdr = Int32.Parse ("6");//nvhdr Header version <6 old

				array5 = new int[2];
				for (int i=0; i<2; i++) {
					array5 [i] = Int32.Parse ("-12345");
				}

				npts = Int32.Parse ((time / 10).ToString ());

				array6 = new int[5];
				for (int i=0; i<5; i++) {
					array6 [i] = Int32.Parse ("-12345");
				}

				iftype = Int32.Parse ("1");

				array7 = new int[24];
				for (int i=0; i<24; i++) {
					array7 [i] = Int32.Parse ("-12345");
				}

				System.Text.StringBuilder sb;

				string kstnm = "RaspMI";
				sb = new System.Text.StringBuilder (kstnm);
				sb.Append (' ', 8 - kstnm.Length);
				kstnmchar = new char[8];
				kstnmchar = sb.ToString ().ToCharArray ();

				string kevnm = filename;
				sb = new System.Text.StringBuilder (kevnm);
				sb.Append (' ', 16 - kevnm.Length);
				kevnmchar = new char[16];
				kevnmchar = sb.ToString ().ToCharArray ();

				string khole = "-12345";
				sb = new System.Text.StringBuilder (khole);
				sb.Append (' ', 8 - khole.Length);
				kholechar = new char[8];
				kholechar = sb.ToString ().ToCharArray ();

				string ko = "-12345";
				sb = new System.Text.StringBuilder (ko);
				sb.Append (' ', 8 - ko.Length);
				kochar = new char[8];
				kochar = sb.ToString ().ToCharArray ();

				string ka = "-12345";
				sb = new System.Text.StringBuilder (ka);
				sb.Append (' ', 8 - ka.Length);
				kachar = new char[8];
				kachar = sb.ToString ().ToCharArray ();

				string kt = "-12345";
				sb = new System.Text.StringBuilder (kt);
				sb.Append (' ', 80 - kt.Length);
				ktchar = new char[80];
				ktchar = sb.ToString ().ToCharArray ();

				string kf = "-12345";
				sb = new System.Text.StringBuilder (kf);
				sb.Append (' ', 8 - kf.Length);
				kfchar = new char[8];
				kfchar = sb.ToString ().ToCharArray ();

				string kuser = "-12345";
				sb = new System.Text.StringBuilder (kuser);
				sb.Append (' ', 24 - kuser.Length);
				kuserchar = new char[24];
				kuserchar = sb.ToString ().ToCharArray ();

				string kcmpnm = comp;//Z Vertical,N North, E East {TRZ, TRN, TRE}
				sb = new System.Text.StringBuilder (kcmpnm);
				sb.Append (' ', 8 - kcmpnm.Length);
				kcmpnmchar = new char[8];
				kcmpnmchar = sb.ToString ().ToCharArray ();

				string knetwk = "-12345";
				sb = new System.Text.StringBuilder (knetwk);
				sb.Append (' ', 8 - knetwk.Length);
				knetwkchar = new char[8];
				knetwkchar = sb.ToString ().ToCharArray ();

				string kdatrd = "-12345";
				sb = new System.Text.StringBuilder (kdatrd);
				sb.Append (' ', 8 - kdatrd.Length);
				kdatrdchar = new char[8];
				kdatrdchar = sb.ToString ().ToCharArray ();

				string kinst = "RaspMI";
				sb = new System.Text.StringBuilder (kinst);
				sb.Append (' ', 8 - kinst.Length);
				kinstchar = new char[8];
				kinstchar = sb.ToString ().ToCharArray ();
			}

			public void createSACFile (sacHeader sH, double[] data)
			{
				string file;
				if (sH.component == "TRZ")
					file = "Data/" + sH.filename + "_Z.SAC";
				else if (sH.component == "TRN")
					file = "Data/" + sH.filename + "_N.SAC";
				else
					file = "Data/" + sH.filename + "_E.SAC";

				System.IO.FileStream fs1 = new FileStream (file, System.IO.FileMode.Create);
				System.IO.BinaryWriter br1 = new BinaryWriter (fs1);
				br1.Write (sH.sampleRate);
				for (int i=0; i<sH.array1.Length; i++)
					br1.Write (sH.array1 [i]);
				br1.Write (sH.b);
				br1.Write (sH.e);
				for (int i=0; i<sH.array2.Length; i++)
					br1.Write (sH.array2 [i]);
				for (int i=0; i<sH.array3.Length; i++)
					br1.Write (sH.array3 [i]);
				for (int i=0; i<sH.array4.Length; i++)
					br1.Write (sH.array4 [i]);
				br1.Write (sH.nvhdr);
				for (int i=0; i<sH.array5.Length; i++)
					br1.Write (sH.array5 [i]);
				br1.Write (sH.npts);
				for (int i=0; i<sH.array6.Length; i++)
					br1.Write (sH.array6 [i]);
				br1.Write (sH.iftype);
				for (int i=0; i<sH.array7.Length; i++)
					br1.Write (sH.array7 [i]);
				br1.Write (sH.kstnmchar);
				br1.Write (sH.kevnmchar);
				br1.Write (sH.kholechar);
				br1.Write (sH.kochar);
				br1.Write (sH.kachar);
				br1.Write (sH.ktchar);
				br1.Write (sH.kfchar);
				br1.Write (sH.kuserchar);
				br1.Write (sH.kcmpnmchar);
				br1.Write (sH.knetwkchar);
				br1.Write (sH.kdatrdchar);
				br1.Write (sH.kinstchar);
				foreach (float da in data) {
					br1.Write (da);
				}
				br1.Close ();
				fs1.Close ();
			}
		}

		protected void OnGPSbtnClicked (object sender, EventArgs e)
		{
			try {
				string portname = "/dev/ttyUSB0";
				gpsSp = new System.IO.Ports.SerialPort ();
				gpsSp.PortName = portname;
				gpsSp.BaudRate = 9600;
				gpsSp.Open ();
				GPSlbl.Text = "GPS : Ready";

				string s = gpsSp.ReadLine ();
				string[] ss = s.Split (',');

			if (ss [0] == "$$GPRMC") {
					if (ss [2] == "V") {
						GPSlbl.Text = "GPS : No Satellite Data";
					} else {
						Ttime = ss [1];
						Tlat = ss [3];
						Tlon = ss [5];
						Tdate = ss [9];

						GPStxt = "Time:" + ss [1] + " Lat:" + ss [3] + " Lon:" + ss [5] + " Date:" + ss [9];
					GPSlbl.Text = GPStxt;
					}
				} else {
					GPSlbl.Text = "GPS : Corrupted Data";
				}
				gpsSp.Close ();
			} catch {
				GPSlbl.Text = "GPS : Not Ready";
				if (gpsSp.IsOpen)
					gpsSp.Close ();
			}
		}

		protected void OnStartbtnClicked (object sender, EventArgs e)
		{
			if (timecombo.ActiveText != "Time") {
				switch (timecombo.ActiveText) {
				case "1 min":
					dc = 1 * 60 * 100;//data count
					break;
				case "3 min":
					dc = 3 * 60 * 100;
					break;
				case "10 min":
					dc = 10 * 60 * 100;
					break;
				case "30 min":
					dc = 30 * 60 * 100;
					break;
				case "60 min":
					dc = 60 * 60 * 100;
					break;
				default:
					goto fin;
				}

				if (GPStxt != "" && GPStxt != null) {
					if (Ttime != null && Tdate != null && Tlat != null && Tlon != null) {			
						if (Ttime != "" && Tdate != "" && Tlat != "" && Tlon != "") {
							fileName = Tdate + Ttime.Substring (0, 6);
						}
					}
				} else {
					tar = DateTime.Now;
					fileName = tar.Day.ToString ("00") + tar.Month.ToString ("00") + tar.Year.ToString ("00") + tar.Hour.ToString ("00") + tar.Minute.ToString ("00") + tar.Second.ToString ("00");
				}

				if (!Directory.Exists ("Data"))
					Directory.CreateDirectory ("Data");

				sw = new StreamWriter ("Data/" + fileName + ".txt");
				sw.AutoFlush = true;

				cnt = 10;
				count = 0;
				timercount = 0;

				timer1 = new Timer ();
				timer1.Interval = 1000;
				timer1.Elapsed += new ElapsedEventHandler (timer1_tick);

				timer2 = new Timer ();
				timer2.Interval = 1000;
				timer2.Elapsed += new ElapsedEventHandler (timer2_tick);

				startbtn.Sensitive = false;
				stopbtn.Sensitive = true;
				connectbtn.Sensitive = false;

				drw = plotdrwarea.Allocation.Width;
				drh = plotdrwarea.Allocation.Height;
				countdown = 10;
				timer2.Start ();				
			} else {
				MessageDialog dialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close, " Please select the recording time");
				dialog.Run ();
				dialog.Destroy ();
			}
			fin:
			;
		}

		public void timer1_tick (object sender, ElapsedEventArgs e)
		{
			Gtk.Application.Invoke (delegate {
				timeinfodrwarea.GdkWindow.Clear ();
				g2.MoveTo (20, 20);
				g2.ShowText ("Remaining Time : " + ((dc / 100) - timercount).ToString ("0000") + " sec.");
				g2.Stroke ();
			});
			timercount++;
		}

		int countdown = 10;

		public void timer2_tick (object sender, ElapsedEventArgs e)
		{
			Gtk.Application.Invoke (delegate {
				timeinfodrwarea.GdkWindow.Clear ();
				g2.MoveTo (20, 20);
				g2.ShowText ("Countdown : " + countdown + " sec.");
				g2.Stroke ();
				if (countdown == 0) {
				devicelbl.Text = "Device : Recording";
					this.timeinfodrwarea.ModifyBg (StateType.Normal, new Gdk.Color (0, 255, 0));
					timercount = 0;
					timer1.Start ();
					timer2.Stop ();
					rec = true;
				}
				countdown--;
			});
		}

		protected void OnStopbtnClicked (object sender, EventArgs e)
		{
			timer1.Stop ();

			if (sw != null) {
				sw.Close ();
				sw = null;
			}

			rec = false;
			save = false;
			data3a.Clear ();
			data3b.Clear ();
			data3c.Clear ();
			save = true;

			stopbtn.Sensitive = false;
			System.Threading.Thread.Sleep (10);
			startbtn.Sensitive = true;
			System.Threading.Thread.Sleep (10);
			connectbtn.Sensitive = true;
			System.Threading.Thread.Sleep (10);

			if (deviceSp.IsOpen)
				deviceSp.Write ("3");

			devicelbl.Text = "Device : Ready";
			timeinfodrwarea.GdkWindow.Clear ();
			this.timeinfodrwarea.ModifyBg (StateType.Normal, new Gdk.Color (255, 0, 0));			
		}		

	int say = 0;

	protected void Update(){
		if (!pause) {
			using (Cairo.Context g3=Gdk.CairoHelper.Create(plotdrwarea.GdkWindow)) {
					data_a = data3a;
					data_b = data3b;
					data_c = data3c;

					g3.SetFontSize (16);
					g3.LineWidth = 0.9;
					g3.SetSourceRGB (0, 255, 0);

					for (int i = 0; i < 100; i++) {
						if (v == 0) {
							p1 = new PointD (0, drh / 6f);
							p2 = new PointD (vv, (float)drh / 3f - ((float)((int)data_a [i] * ((float)drh / 3f / 4095.0))));

							p1b = new PointD (0, (float)drh / 2f);
							p2b = new PointD (vv, 2 * (float)drh / 3f - (((float)((int)data_b [i] * ((float)drh / 3f / 4095.0)))));

							p1c = new PointD (0, 5 * (float)drh / 6f);
							p2c = new PointD (vv, (float)drh - (((float)((int)data_c [i] * ((float)drh / 3f / 4095.0)))));
						} else {
							p2 = new PointD (v, (float)drh / 3f - ((float)((int)data_a [i] * ((float)drh / 3f / 4095.0))));
							p2b = new PointD (v, 2 * (float)drh / 3f - ((float)((int)data_b [i] * ((float)drh / 3f / 4095.0))));
							p2c = new PointD (v, (float)drh - ((float)((int)data_c [i] * ((float)drh / 3f / 4095.0))));

							g3.MoveTo (p1);
							g3.LineTo (p2);
							g3.MoveTo (p1b);
							g3.LineTo (p2b);
							g3.MoveTo (p1c);
							g3.LineTo (p2c);

							p1 = p2;
							p1b = p2b;
							p1c = p2c;
							if (v >= drw) {
							v = 0;
							say=-1;

								p1 = new PointD (0, (float)((int)data_a [i] * ((float)drh / 3f / 4095.0)));
								p2 = new PointD (0, (float)((int)data_a [i] * ((float)drh / 3f / 4095.0)));
								p1b = new PointD (0, (float)((int)data_b [i] * ((float)drh / 3f / 4095.0)) + (float)drh / 3f);
								p2b = new PointD (0, (float)((int)data_b [i] * ((float)drh / 3f / 4095.0)) + (float)drh / 3f);
								p1c = new PointD (0, (float)((int)data_c [i] * ((float)drh / 3f / 4095.0)) + 2 * (float)drh / 3f);
								p2c = new PointD (0, (float)((int)data_c [i] * ((float)drh / 3f / 4095.0)) + 2 * (float)drh / 3f);

							}
						}
						v += vv;
					}

					data3a.Clear ();
					data3b.Clear ();
					data3c.Clear ();

					g3.Stroke ();
					
					g3.SetSourceRGB (0, 0, 0);

				if ((v-vv + 100) <= drw) {
					g3.Rectangle(v-vv, 0, 100, drh);
				} else {
					g3.Rectangle(v-vv, 0, 100, drh);
					g3.Rectangle(0, 0, v-vv+100-800, drh);
				}

				g3.FillPreserve ();
				g3.Stroke ();

				g3.SetSourceRGB (0, 255, 0);

				if (say == 0) {
					g3.MoveTo (10, 20);
					g3.ShowText ("Vertical");
					g3.MoveTo (10, drh / 3.0 + 20);
					g3.ShowText ("North-South");
					g3.MoveTo (10, 2 * drh / 3.0 + 20);
					g3.ShowText ("East-West");
					g3.Stroke ();
				}

				say++;

					g3.GetTarget ().Dispose ();
					g3.Dispose ();

					data_a.Clear ();
					data_b.Clear ();
					data_c.Clear ();
			}
		}
	}
}