using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D; 


namespace WindowsClock
{
    public partial class FormClock : Form
    {
        private HatchBrush hb;
        private Pen p, ps, pm, ph, pt;
        private float x, y;
        private double angs = 0, angm = 0, angh = 0;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public FormClock()
        {
            InitializeComponent();
        }

        private void MAJ()
        {
            DateTime dt = DateTime.Now;

            angh = (2 * Math.PI) * ((float)((dt.Hour % 12) +
                ((float)(dt.Minute) / 60)) / 12);
            angm = (2 * Math.PI) * ((float)(dt.Minute) / 60);
            textBoxDay.Text = dt.DayOfWeek.ToString();

        }


        private void FormClock_Load(object sender, EventArgs e)
        {
            MAJ();
            
            hb = new HatchBrush(HatchStyle.LargeGrid, Color.LightBlue, Color.MediumSeaGreen);
            p = new Pen(hb,24);
            ps = new Pen(Color.Cyan, 2);
            pm = new Pen(Color.Black, 4);
            ph = new Pen(Color.Red, 8);
            pt = new Pen(Color.Silver, 2);

            x = (float)this.Width / 2;
            y = (float)this.Height / 2;       
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            if (angh <= 0.00000001)
                MAJ();
            angs += (2 * Math.PI) / 60;
            this.Invalidate();
        }

        private void FormClock_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(x, y);

            Point ptF;
            int i = 0; 
            for (float angt = 0; angt <= 2 * Math.PI; angt += (float)(2 * Math.PI) / 12)
            {
                Point pt1 = new Point(Convert.ToInt32(150 * Math.Sin(angt)),
                    -Convert.ToInt32(150 * Math.Cos(angt)));
                Point pt2 = new Point(Convert.ToInt32(175 * Math.Sin(angt)),
                    -Convert.ToInt32(175 * Math.Cos(angt)));

                e.Graphics.DrawLine(pt, pt1, pt2);

                if (i != 0)
                {
                    ptF = pt1;
                    ptF.X -= 6;
                    ptF.Y -= 6;
                    e.Graphics.DrawString(i.ToString(),
                      new Font("Arial", 15),
                      new SolidBrush(Color.Black),
                      ptF);
                }
                i++;
            }
        
            e.Graphics.DrawEllipse(p, new Rectangle(-188, -188, 376, 376));

            Point p2 = new Point(Convert.ToInt32(172 * Math.Sin(angs)),
               -Convert.ToInt32(172 * Math.Cos(angs)));
            e.Graphics.DrawLine(ps, new Point(0, 0), p2);

            Point p3 = new Point(Convert.ToInt32(172 * Math.Sin(angm)),
               -Convert.ToInt32(172 * Math.Cos(angm)));
            e.Graphics.DrawLine(pm, new Point(0, 0), p3);

            Point p4 = new Point(Convert.ToInt32(130 * Math.Sin(angh)),
             -Convert.ToInt32(130 * Math.Cos(angh)));
            e.Graphics.DrawLine(ph, new Point(0, 0), p4);
        }

        private void FormClock_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Alt == true) && (e.KeyCode == Keys.F3))
                Close();
        }

        private void FormClock_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }


    }
}
