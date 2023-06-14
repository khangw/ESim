using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicSimulator.Components
{
    public class Res : Com
    {
        public int width = 110;
        public int height = 80;
        public Res(int x, int y)
        {
            this.x = x;this.y = y;
            value = 10;
        }

        public override void Draw(Graphics graphics)
        {
            PointF[] points = {new Point(x, y), new Point(x + 5*len/4, y), new Point(x + 3*len/2, y - len), new Point(x + 2*len, y + len),
                               new Point(x + 5*len/2, y - len), new Point(x + 3*len, y + len), new Point(x + 7*len/2, y - len), new Point(x + 4*len, y + len),
                               new Point(x + 9*len/2 - len/4, y), new Point(x + 11*len/2, y)};
            graphics.DrawLines(pen, points);
            graphics.DrawEllipse(penpoint, x - 20, y - 10, 20, 20);
            graphics.DrawEllipse(penpoint, x + 11*len/2, y - 10, 20, 20);
        }
        public override bool Contains(Point location)
        {

            Rectangle boundingRect = new Rectangle(x, y - 40, width, height);
            return boundingRect.Contains(location);

        }
        public override bool Connec(Point location)
        {
            Rectangle boudingRect1 = new Rectangle(x - 20, y - 10, 20, 20);
            Rectangle boudingRect2 = new Rectangle(x + 11 * len / 2, y - 10, 20, 20);
            return boudingRect1.Contains(location) || boudingRect2.Contains(location);
        }
        public override Com Clone()
        {
            return new Res(x + 20, y + 20);
        }
        public override void Rotate(int angle)
        {
            RotationAngle = angle;
        }
    }
}
