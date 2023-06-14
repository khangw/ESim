using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicSimulator.Components
{
    public class Vol : Com
    {
        public int size = 25;
        public Vol(int x, int y)
        {
            this.x = x; this.y = y;
            value = 5.0;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawEllipse(pen, x, y, size, size);
            graphics.DrawString("5V", font, brush, new Point(x, y + 30));
        }
        public override bool Contains(Point location)
        {
            
            Rectangle boundingRect = new Rectangle(x, y, size, size);
            return boundingRect.Contains(location);
            
        }
        public override bool Connec(Point location)
        {
            Rectangle boudingRect = new Rectangle(x, y, size, size);
            return boudingRect.Contains(location);
        }
        public override Com Clone()
        {
            return new Vol(x + 20, y + 20);
        }
        public override void Rotate(int angle)
        {

        }
    }
}