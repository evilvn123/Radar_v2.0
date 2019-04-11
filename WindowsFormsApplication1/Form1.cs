using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;


namespace WindowsFormsApplication1
{
    //class Data_Thread
    //{
    //    string dataIn;
    //    SerialPort serialPort1;

    //    delegate void SetTextCallback(string text);
    //    public void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
    //    {
    //        dataIn = serialPort1.ReadExisting();
    //        if (dataIn != String.Empty)
    //        {
    //            SetText(dataIn);
    //        }
    //    }
    //    public void SetText(string text)
    //    {

    //        if (this.dataIn.InvokeRequired)
    //        {

    //            SetTextCallback d = new SetTextCallback(SetText);

    //            this.Invoke(d, new object[] { text });

    //        }

    //        else this.dataIn = text;

    //    }
    //}
    public partial class Form1 : Form
    {

        string dataIn;

        Graphics g;
        Pen p,aBrush;
        Pen p1;
        Point[] pnt = new Point[3];
        Point[] pnt1 = new Point[3];
        Brush brush  = new SolidBrush(Color.Green);
        Brush brush1 = default(Brush);
        int centerpointx;
        int centerpointy;
        bool check = true;
        private int angle1 = 5;
        private int angle2;
        private int l_pen = 400;
        private int scan_angle;
        private int radius = 360;

        delegate void SetTextCallback(string text);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            brush1 =new SolidBrush(this.BackColor);
            timer1.Stop();
            cboComPort.DataSource = SerialPort.GetPortNames();
            btnOpen.Enabled = true;
            btnClose.Enabled = false;
        }

        public double DETOGRAD(int deg)
        {
            return Math.PI * deg / 180;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
                 
                if (check)
                {
                    Scan_Clock();
                }

                if (!check)
                {
                    Scan_ReverseClock();
                }
           

            lblAngle.Text = angle1 + "°";
        }
        private void Scan_Clock()
        {
            scan_angle = 4;
            angle2 = (angle1 - scan_angle);
            p = new Pen(Color.GreenYellow, 5);   //for color          
            
            Decoration();            
            pnt[0].X = centerpointx;
            pnt[0].Y = centerpointy;
            pnt[1].X=(int)(centerpointx + radius*((float)Math.Cos(DETOGRAD(angle1))));
            pnt[1].Y=(int)(centerpointy - radius*((float)Math.Sin(DETOGRAD(angle1))));
            pnt[2].X=(int)(centerpointx + radius*((float)Math.Cos(DETOGRAD(angle1+2))));
            pnt[2].Y=(int)(centerpointy - radius*((float)Math.Sin(DETOGRAD(angle1+2))));
            Triangle(p,brush, pnt[0], pnt[1], pnt[2]);


            p1 = new Pen(this.BackColor, 5);
            pnt1[0].X = centerpointx;
            pnt1[0].Y = centerpointy;
            pnt1[1].X = (int)(centerpointx + radius * ((float)Math.Cos(DETOGRAD(angle2))));
            pnt1[1].Y = (int)(centerpointy - radius * ((float)Math.Sin(DETOGRAD(angle2))));
            pnt1[2].X = (int)(centerpointx + radius * ((float)Math.Cos(DETOGRAD(angle2 + 2))));
            pnt1[2].Y = (int)(centerpointy - radius * ((float)Math.Sin(DETOGRAD(angle2 + 2))));
            Triangle(p1,brush1, pnt1[0], pnt1[1], pnt1[2]);
            
            angle1++;
            Convert_data();
            if (angle1 == 179)
            {
                angle1 = 176;
                check = false; 
            }
        }
        private void Scan_ReverseClock()
        {
            scan_angle = -4;
            angle2 = (angle1 - scan_angle);
            p = new Pen(Color.GreenYellow, 5);   //for color
            Decoration();
            pnt[0].X = centerpointx;
            pnt[0].Y = centerpointy;
            pnt[1].X = (int)(centerpointx + radius * ((float)Math.Cos(DETOGRAD(angle1))));
            pnt[1].Y = (int)(centerpointy - radius * ((float)Math.Sin(DETOGRAD(angle1))));
            pnt[2].X = (int)(centerpointx + radius * ((float)Math.Cos(DETOGRAD(angle1 - 2))));
            pnt[2].Y = (int)(centerpointy - radius * ((float)Math.Sin(DETOGRAD(angle1 - 2))));
            Triangle(p,brush, pnt[0], pnt[1], pnt[2]);

            p1 = new Pen(this.BackColor, 5);
            pnt1[0].X = centerpointx;
            pnt1[0].Y = centerpointy;
            pnt1[1].X = (int)(centerpointx + radius * ((float)Math.Cos(DETOGRAD(angle2))));
            pnt1[1].Y = (int)(centerpointy - radius * ((float)Math.Sin(DETOGRAD(angle2))));
            pnt1[2].X = (int)(centerpointx + radius * ((float)Math.Cos(DETOGRAD(angle2 - 2))));
            pnt1[2].Y = (int)(centerpointy - radius * ((float)Math.Sin(DETOGRAD(angle2 - 2))));
            Triangle(p1,brush1, pnt1[0], pnt1[1], pnt1[2]);
            
            angle1--;
            Convert_data();
            if (angle1 == 0) 
            { 
                angle1 = 3;  
                check = true; 
            }

        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            lbl0.Visible = lbl30.Visible = lbl60.Visible = lbl90.Visible = lbl120.Visible = lbl150.Visible = lbl180.Visible = true;
            lbldistance1.Visible = lbldistance2.Visible = lbldistance3.Visible = lbldistance4.Visible = true;
            label1.Visible = label5.Visible = lblAngle.Visible = lblDistance0.Visible = true;
        }
        private void Decoration()
        {
            //intial
            g = this.CreateGraphics();
            p = new Pen(Brushes.Green, 3);
            centerpointx = this.Width / 2 - 150;
            centerpointy = this.Height - 90;
            //draw circle
            g.DrawEllipse(p, centerpointx - radius, centerpointy - radius, 720, 720);
            g.DrawEllipse(p, centerpointx - 270, centerpointy - 270, 540, 540);
            g.DrawEllipse(p, centerpointx - 180, centerpointy - 180, 360, 360);
            g.DrawEllipse(p, centerpointx - 90, centerpointy - 90, 180, 180);

            PointF p1 = new PointF();
            PointF p2 = new PointF();
            PointF p3 = new PointF();
            PointF p4 = new PointF();
            PointF p5 = new PointF();
            PointF p6 = new PointF();
            PointF p7 = new PointF();
            PointF p8 = new PointF();
            p1.X = centerpointx - l_pen;
            p1.Y = centerpointy;
            p2.X = centerpointx + l_pen;
            p2.Y = centerpointy;
            p3.X = centerpointx;
            p3.Y = centerpointy;
            p4.X = centerpointx;
            p4.Y = centerpointy - l_pen;
            p5.X = centerpointx - ((float)Math.Cos(DETOGRAD(30)) * l_pen);
            p5.Y = centerpointy - ((float)Math.Sin(DETOGRAD(30)) * l_pen);
            p6.X = centerpointx + ((float)Math.Cos(DETOGRAD(30)) * l_pen);
            p6.Y = centerpointy - ((float)Math.Sin(DETOGRAD(30)) * l_pen);
            p7.X = centerpointx - ((float)Math.Cos(DETOGRAD(60)) * l_pen);
            p7.Y = centerpointy - ((float)Math.Sin(DETOGRAD(60)) * l_pen);
            p8.X = centerpointx + ((float)Math.Cos(DETOGRAD(60)) * l_pen);
            p8.Y = centerpointy - ((float)Math.Sin(DETOGRAD(60)) * l_pen);
            g.DrawLine(p, p1, p2);
            g.DrawLine(p, p3, p4);
            g.DrawLine(p, p3, p5);
            g.DrawLine(p, p3, p6);
            g.DrawLine(p, p3, p7);
            g.DrawLine(p, p3, p8);
        }

        #region buttonchange
        private void btnStart_MouseEnter(object sender, EventArgs e)
        {
            btnStart.Size = new Size(60, 60);
        }
        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            btnStart.Size = new Size(50, 50);
        }
        private void btnStop_MouseEnter(object sender, EventArgs e)
        {
            btnStop.Size = new Size(60, 60);
        }
        private void btnStop_MouseLeave(object sender, EventArgs e)
        {
            btnStop.Size = new Size(50, 50);
        }
        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            btnExit.Size = new Size(60, 60);
        }
        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.Size = new Size(50, 50);
        }

        #endregion


        private void btnExit_Click(object sender, EventArgs e)
        {
                     
            DialogResult ret = MessageBox.Show("Bạn muốn thoát ứng dụng ?", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            serialPort1.Close();
            Close();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value == 1)
                timer1.Interval = 1000;
            if (trackBar1.Value == 2)
                timer1.Interval = 500;
            if (trackBar1.Value == 3)
                timer1.Interval = 400;
            if (trackBar1.Value == 4)
                timer1.Interval = 300;
            if (trackBar1.Value == 5)
                timer1.Interval = 200;
            if (trackBar1.Value == 6)
                timer1.Interval = 100;
            if (trackBar1.Value == 7)
                timer1.Interval = 60;
            if (trackBar1.Value == 8)
                timer1.Interval = 30;
            if (trackBar1.Value == 9)
                timer1.Interval = 10;
            if (trackBar1.Value == 10)
                timer1.Interval = 1;
        }
        private void txtRadius_ValueChanged(object sender, EventArgs e)
        {
            lbldistance1.Text = ((float)txtRadius.Value / 4).ToString() + " cm";
            lbldistance2.Text = ((float)txtRadius.Value / 2).ToString() + " cm";
            lbldistance3.Text = ((float)txtRadius.Value * 3 / 4).ToString() + " cm";
            lbldistance4.Text = ((float)txtRadius.Value).ToString() + " cm";
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult ret = MessageBox.Show("Ban co muon thoat ?", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = (ret == DialogResult.No);

        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cboComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cboBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32("8");
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                serialPort1.Open();
                progressBar1.Value = 100;

                btnOpen.Enabled = false;
                btnClose.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {                
                progressBar1.Value = 0;
                serialPort1.Close();
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
            }
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                dataIn = serialPort1.ReadLine();
                SetText(dataIn);
            }
            catch
            {

            }
        }
        private void SetText(string text)
        {

            if (this.lblDistance0.InvokeRequired)
            {

                SetTextCallback d = new SetTextCallback(SetText); // khởi tạo 1 delegate mới gọi đến SetText

                this.Invoke(d, new object[] { text });               
            }

            else this.lblDistance0.Text = text;

        }
        private void ShowData(object sender, EventArgs e)
        {
            lblDistance0.Text = dataIn;
        }
        private void Convert_data()
        {
            if (serialPort1.IsOpen) 
            {
                try
                {
                    float distance_object = int.Parse(dataIn);
                    int distance_scan = Convert.ToInt32(txtRadius.Value);
                    float OA = radius * (float)(distance_object / distance_scan);

                    if (OA < radius)
                    {
                        Point p = new Point();
                        p.X = (int)centerpointx + (int)(OA * Math.Cos(Math.PI * angle2 / 180));
                        p.Y = (int)centerpointy - (int)(OA * Math.Sin(Math.PI * angle2 / 180));

                        aBrush = new Pen(Color.Red, 3);
                        g.DrawRectangle(aBrush, p.X, p.Y, 2, 2);
                    }
                    lblDistance0.Text = "";
                }
                catch { }
            }
            
            
        }

        private void Triangle(Pen pen_draw,Brush b, Point p1, Point p2, Point p3)
        {          
            Point[] pnt = new Point[3];

            pnt[0] = p1;
            pnt[1] = p2;
            pnt[2] = p3;


            g.DrawPolygon(pen_draw, pnt);

            g.FillPolygon(b, pnt);
           
        }        
    }
}
