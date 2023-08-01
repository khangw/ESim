using ElectronicSimulator.Coms;
using System.Collections.Generic;
using System.Drawing;

namespace ElectronicSimulator.Components
{
    public class DCVol : Com
    {
        public DCVol(int x, int y)
        {
            connectPoint = new List<Point>();
            connectWire = new List<Wire>();
            this.x = x; this.y = y;
            value = 1;
            connectPoint.Add(new Point(x - 40, y));//catot
            connectPoint.Add(new Point(x + 40, y));//anot
        }

        public override void draw(Graphics graphics)
        {
            if (isRotate)
            {
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 30, y - 50, 60, 100));

                graphics.DrawString("~V" + name, font, brush, new Point(x - 40, y - 10));
                graphics.DrawString(value + "V", font, brush, new Point(x + 20, y - 10));

                graphics.DrawEllipse(pen, x - 4, y - 43, psize, psize);
                graphics.DrawEllipse(pen, x - 4, y + 37, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y - 43, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y + 37, psize, psize);

                graphics.DrawLine(pen, new Point(x, y - 37), new Point(x, y - 20));
                graphics.DrawLine(pen, new Point(x, y + 37), new Point(x, y + 20));
                graphics.DrawLine(pen, new Point(x - 20, y - 20), new Point(x + 20, y - 20));
                graphics.DrawLine(pen, new Point(x - 10, y - 6), new Point(x + 10, y - 6));
                graphics.DrawLine(pen, new Point(x - 20, y + 6), new Point(x + 20, y + 6));
                graphics.DrawLine(pen, new Point(x - 10, y + 20), new Point(x + 10, y + 20));
            }
            else
            {
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 50, y - 30, 100, 60));

                graphics.DrawString("~V" + name, font, brush, new Point(x - 10, y - 40));
                graphics.DrawString(value + "V", font, brush, new Point(x - 10, y + 20));

                graphics.DrawEllipse(pen, x - 43, y - 4, psize, psize);
                graphics.DrawEllipse(pen, x + 37, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x - 43, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x + 37, y - 4, psize, psize);

                graphics.DrawLine(pen, new Point(x - 37, y), new Point(x - 20, y));
                graphics.DrawLine(pen, new Point(x + 37, y), new Point(x + 20, y));
                graphics.DrawLine(pen, new Point(x - 20, y - 20), new Point(x - 20, y + 20));
                graphics.DrawLine(pen, new Point(x - 6, y - 10), new Point(x - 6, y + 10));
                graphics.DrawLine(pen, new Point(x + 6, y - 20), new Point(x + 6, y + 20));
                graphics.DrawLine(pen, new Point(x + 20, y - 10), new Point(x + 20, y + 10));
            }
        }
        public override void updateConnectPoints()
        {
            if (isRotate)
            {
                connectPoint[0] = new Point(x, y - 40);
                connectPoint[1] = new Point(x, y + 40);
            }
            else
            {
                connectPoint[0] = new Point(x - 40, y);
                connectPoint[1] = new Point(x + 40, y);
            }
        }

        public override bool contains(Point location)
        {
            if (isRotate)
            {
                Rectangle boundingRect = new Rectangle(x - 30, y - 50, 60, 100);
                return boundingRect.Contains(location);
            }
            else
            {
                Rectangle boundingRect = new Rectangle(x - 50, y - 30, 100, 60);
                return boundingRect.Contains(location);
            }
        }
        public override Point connects(Point location)
        {
            if (isRotate)
            {
                Rectangle boudingRect1 = new Rectangle(x - 3, y - 43, psize, psize);
                Rectangle boudingRect2 = new Rectangle(x - 3, y + 37, psize, psize);
                if (boudingRect1.Contains(location)) return connectPoint[0];
                else if (boudingRect2.Contains(location)) return connectPoint[1];
                else return Point.Empty;
            }
            else
            {
                Rectangle boudingRect1 = new Rectangle(x - 43, y - 3, psize, psize);
                Rectangle boudingRect2 = new Rectangle(x + 37, y - 3, psize, psize);
                if (boudingRect1.Contains(location)) return connectPoint[0];
                else if (boudingRect2.Contains(location)) return connectPoint[1];
                else return Point.Empty;
            }
        }
        public override Com clone()
        {
            return new DCVol(x + 20, y + 20);
        }
        public override Com stateClone()
        {
            DCVol dcVol = new DCVol(x, y);
            dcVol.name = name;
            dcVol.value = value;
            return dcVol;
        }
    }
}