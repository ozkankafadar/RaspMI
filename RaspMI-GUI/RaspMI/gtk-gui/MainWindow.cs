
// This file has been generated by the GUI designer. Do not modify.
public partial class MainWindow
{
	private global::Gtk.Table table2;
	private global::Gtk.DrawingArea plotdrwarea;
	private global::Gtk.Table table3;
	private global::Gtk.Table table4;
	private global::Gtk.Button connectbtn;
	private global::Gtk.Button GPSbtn;
	private global::Gtk.Button startbtn;
	private global::Gtk.Button stopbtn;
	private global::Gtk.ComboBoxEntry timecombo;
	private global::Gtk.DrawingArea timeinfodrwarea;
	private global::Gtk.Table table5;
	private global::Gtk.Label devicelbl;
	private global::Gtk.Label GPSlbl;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("RaspMI");
		this.WindowPosition = ((global::Gtk.WindowPosition)(3));
		this.DefaultWidth = 794;
		this.DefaultHeight = 476;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.table2 = new global::Gtk.Table (((uint)(2)), ((uint)(1)), false);
		this.table2.Name = "table2";
		this.table2.RowSpacing = ((uint)(6));
		this.table2.ColumnSpacing = ((uint)(6));
		// Container child table2.Gtk.Table+TableChild
		this.plotdrwarea = new global::Gtk.DrawingArea ();
		this.plotdrwarea.Name = "plotdrwarea";
		this.table2.Add (this.plotdrwarea);
		global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table2 [this.plotdrwarea]));
		w1.TopAttach = ((uint)(1));
		w1.BottomAttach = ((uint)(2));
		// Container child table2.Gtk.Table+TableChild
		this.table3 = new global::Gtk.Table (((uint)(2)), ((uint)(1)), false);
		this.table3.Name = "table3";
		this.table3.RowSpacing = ((uint)(6));
		this.table3.ColumnSpacing = ((uint)(6));
		// Container child table3.Gtk.Table+TableChild
		this.table4 = new global::Gtk.Table (((uint)(1)), ((uint)(6)), false);
		this.table4.Name = "table4";
		this.table4.RowSpacing = ((uint)(6));
		this.table4.ColumnSpacing = ((uint)(6));
		// Container child table4.Gtk.Table+TableChild
		this.connectbtn = new global::Gtk.Button ();
		this.connectbtn.CanFocus = true;
		this.connectbtn.Name = "connectbtn";
		this.connectbtn.UseUnderline = true;
		this.connectbtn.Label = global::Mono.Unix.Catalog.GetString ("CONNECT");
		this.table4.Add (this.connectbtn);
		global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table4 [this.connectbtn]));
		w2.LeftAttach = ((uint)(2));
		w2.RightAttach = ((uint)(3));
		w2.XOptions = ((global::Gtk.AttachOptions)(4));
		w2.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table4.Gtk.Table+TableChild
		this.GPSbtn = new global::Gtk.Button ();
		this.GPSbtn.CanFocus = true;
		this.GPSbtn.Name = "GPSbtn";
		this.GPSbtn.UseUnderline = true;
		this.GPSbtn.Label = global::Mono.Unix.Catalog.GetString ("GPS");
		this.table4.Add (this.GPSbtn);
		global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table4 [this.GPSbtn]));
		w3.LeftAttach = ((uint)(5));
		w3.RightAttach = ((uint)(6));
		w3.XOptions = ((global::Gtk.AttachOptions)(4));
		w3.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table4.Gtk.Table+TableChild
		this.startbtn = new global::Gtk.Button ();
		this.startbtn.CanFocus = true;
		this.startbtn.Name = "startbtn";
		this.startbtn.UseUnderline = true;
		this.startbtn.Label = global::Mono.Unix.Catalog.GetString ("START");
		this.table4.Add (this.startbtn);
		global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table4 [this.startbtn]));
		w4.LeftAttach = ((uint)(3));
		w4.RightAttach = ((uint)(4));
		w4.XOptions = ((global::Gtk.AttachOptions)(4));
		w4.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table4.Gtk.Table+TableChild
		this.stopbtn = new global::Gtk.Button ();
		this.stopbtn.CanFocus = true;
		this.stopbtn.Name = "stopbtn";
		this.stopbtn.UseUnderline = true;
		this.stopbtn.Label = global::Mono.Unix.Catalog.GetString ("STOP");
		this.table4.Add (this.stopbtn);
		global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table4 [this.stopbtn]));
		w5.LeftAttach = ((uint)(4));
		w5.RightAttach = ((uint)(5));
		w5.XOptions = ((global::Gtk.AttachOptions)(4));
		w5.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table4.Gtk.Table+TableChild
		this.timecombo = global::Gtk.ComboBoxEntry.NewText ();
		this.timecombo.AppendText (global::Mono.Unix.Catalog.GetString ("Time"));
		this.timecombo.AppendText (global::Mono.Unix.Catalog.GetString ("1 min"));
		this.timecombo.AppendText (global::Mono.Unix.Catalog.GetString ("3 min"));
		this.timecombo.AppendText (global::Mono.Unix.Catalog.GetString ("10 min"));
		this.timecombo.AppendText (global::Mono.Unix.Catalog.GetString ("30 min"));
		this.timecombo.AppendText (global::Mono.Unix.Catalog.GetString ("60 min"));
		this.timecombo.WidthRequest = 120;
		this.timecombo.Name = "timecombo";
		this.table4.Add (this.timecombo);
		global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table4 [this.timecombo]));
		w6.LeftAttach = ((uint)(1));
		w6.RightAttach = ((uint)(2));
		w6.XOptions = ((global::Gtk.AttachOptions)(4));
		w6.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table4.Gtk.Table+TableChild
		this.timeinfodrwarea = new global::Gtk.DrawingArea ();
		this.timeinfodrwarea.WidthRequest = 100;
		this.timeinfodrwarea.Name = "timeinfodrwarea";
		this.table4.Add (this.timeinfodrwarea);
		global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table4 [this.timeinfodrwarea]));
		w7.YOptions = ((global::Gtk.AttachOptions)(4));
		this.table3.Add (this.table4);
		global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table3 [this.table4]));
		w8.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table3.Gtk.Table+TableChild
		this.table5 = new global::Gtk.Table (((uint)(1)), ((uint)(3)), false);
		this.table5.Name = "table5";
		this.table5.RowSpacing = ((uint)(6));
		this.table5.ColumnSpacing = ((uint)(6));
		// Container child table5.Gtk.Table+TableChild
		this.devicelbl = new global::Gtk.Label ();
		this.devicelbl.WidthRequest = 170;
		this.devicelbl.Name = "devicelbl";
		this.devicelbl.LabelProp = global::Mono.Unix.Catalog.GetString ("Device : Not Connected");
		this.devicelbl.Justify = ((global::Gtk.Justification)(3));
		this.table5.Add (this.devicelbl);
		global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table5 [this.devicelbl]));
		w9.XOptions = ((global::Gtk.AttachOptions)(4));
		w9.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table5.Gtk.Table+TableChild
		this.GPSlbl = new global::Gtk.Label ();
		this.GPSlbl.WidthRequest = 470;
		this.GPSlbl.Name = "GPSlbl";
		this.GPSlbl.LabelProp = global::Mono.Unix.Catalog.GetString ("GPS : Not Connected");
		this.GPSlbl.Justify = ((global::Gtk.Justification)(3));
		this.table5.Add (this.GPSlbl);
		global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table5 [this.GPSlbl]));
		w10.LeftAttach = ((uint)(2));
		w10.RightAttach = ((uint)(3));
		w10.XOptions = ((global::Gtk.AttachOptions)(4));
		w10.YOptions = ((global::Gtk.AttachOptions)(4));
		this.table3.Add (this.table5);
		global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table3 [this.table5]));
		w11.TopAttach = ((uint)(1));
		w11.BottomAttach = ((uint)(2));
		w11.YOptions = ((global::Gtk.AttachOptions)(4));
		this.table2.Add (this.table3);
		global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table2 [this.table3]));
		w12.YOptions = ((global::Gtk.AttachOptions)(4));
		this.Add (this.table2);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.stopbtn.Clicked += new global::System.EventHandler (this.OnStopbtnClicked);
		this.startbtn.Clicked += new global::System.EventHandler (this.OnStartbtnClicked);
		this.GPSbtn.Clicked += new global::System.EventHandler (this.OnGPSbtnClicked);
		this.connectbtn.Clicked += new global::System.EventHandler (this.OnConnectbtnClicked);
	}
}
