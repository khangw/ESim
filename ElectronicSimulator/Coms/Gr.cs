using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicSimulator.Components
{
    public class Gr : Com
    {
        public int width = 50;
        public int height = 50;
        public Gr(int x, int y) 
        {
            this.x = x; this.y = y;
            value = 0;
        }
        public override void Draw(Graphics graphics)
        {
            graphics.DrawEllipse(penpoint, x - 20, y - 10, 20, 20);
            graphics.DrawLine(pen, new Point(x, y), new Point(x + len, y));
            graphics.DrawLine(pen, new Point(x + len, y - 3*len/2), new Point(x + len, y + 3*len/2));
            graphics.DrawLine(pen, new Point(x + 7*len/4, y - len), new Point(x + 7*len/4, y + len));
            graphics.DrawLine(pen, new Point(x + 5*len/2, y - len / 2), new Point(x + 5*len/2, y + len / 2));
        }
        public override bool Contains(Point location)
        {
            Rectangle boundingRect = new Rectangle(x, y - 25, width, height);
            return boundingRect.Contains(location);
        }
        public override bool Connec(Point location)
        {
            Rectangle boudingRect = new Rectangle(x - 20, y - 10, 20, 20);
            return boudingRect.Contains(location);
        }
        public override Com Clone()
        {
            return new Gr(x + 20, y + 20);
        }
        public override void Rotate(int angle)
        {
        }
    }
}
