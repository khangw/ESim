using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicSimulator
{
    public abstract class Com
    {
        public int x { get; set; }
        public int y { get; set; }
        public double value;
        public int RotationAngle { get; set; }
        public Font font = new Font("Arial", 12);
        public Brush brush = Brushes.Black;
        public int len = 20;
        public static int penWidth = 10;
        public Pen pen = new Pen(Color.Black, penWidth);
        public Pen penselec = new Pen(Color.Red, penWidth);
        public Pen penpoint = new Pen(Color.Black, penWidth/5);
        public abstract void Draw(Graphics graphics);
        public abstract bool Contains(Point location);
        public abstract bool Connec(Point location);
        public abstract Com Clone();
        public abstract void Rotate(int angle);
    }
}
