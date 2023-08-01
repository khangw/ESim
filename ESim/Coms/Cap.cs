using ElectronicSimulator.Components;
using System.Collections.Generic;
using System.Drawing;

namespace ElectronicSimulator.Coms
{
    public class Cap : Com
    {
        public Cap(int x, int y)
        {
            connectPoint = new List<Point>();
            connectWire = new List<Wire>();
            this.x = x; this.y = y;
            value = 1;
            connectPoint.Add(new Point(x - 40, y));
            connectPoint.Add(new Point(x + 60, y));
        }

        public override void draw(Graphics graphics)
        {
            if (isRotate)
            {
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 30, y - 50, 60, 120));

                graphics.DrawString("C" + name, font, brush, new Point(x - 40, y));
                graphics.DrawString(value + "μF", font, brush, new Point(x + 20, y));

                graphics.DrawEllipse(pen, x - 4, y - 43, psize, psize);
                graphics.DrawEllipse(pen, x - 4, y + 57, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y - 43, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y + 57, psize, psize);

                graphics.DrawLine(pen, new Point(x, y - 37), new Point(x, y + 5));
                graphics.DrawLine(pen, new Point(x, y + 57), new Point(x, y + 5));
                graphics.DrawLine(pen, new Point(x - 15, y + 5), new Point(x + 15, y + 5));
                graphics.DrawLine(pen, new Point(x - 15, y + 15), new Point(x + 15, y + 15));
                
            }
            else
            {
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 50, y - 30, 120, 60));

                graphics.DrawString("C" + name, font, brush, new Point(x, y - 40));
                graphics.DrawString(value + "μF", font, brush, new Point(x, y + 20));

                graphics.DrawEllipse(pen, x - 43, y - 4, psize, psize);
                graphics.DrawEllipse(pen, x + 57, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x - 43, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x + 57, y - 4, psize, psize);

                graphics.DrawLine(pen, new Point(x - 37, y), new Point(x + 5, y));
                graphics.DrawLine(pen, new Point(x + 57, y), new Point(x + 15, y));
                graphics.DrawLine(pen, new Point(x + 5, y - 15), new Point(x + 5, y + 15));
                graphics.DrawLine(pen, new Point(x + 15, y - 15), new Point(x + 15, y + 15));
            }
        }
        public override void updateConnectPoints()
        {
            if (isRotate)
            {
                connectPoint[0] = new Point(x, y - 40);
                connectPoint[1] = new Point(x, y + 60);
            }
            else
            {
                connectPoint[0] = new Point(x - 40, y);
                connectPoint[1] = new Point(x + 60, y);
            }
        }
        public override bool contains(Point location)
        {
            if (isRotate)
            {
                Rectangle boundingRect = new Rectangle(x - 30, y - 50, 60, 120);
                return boundingRect.Contains(location);
            }
            else
            {
                Rectangle boundingRect = new Rectangle(x - 50, y - 30, 120, 60);
                return boundingRect.Contains(location);
            }
        }
        public override Point connects(Point location)
        {
            if (isRotate)
            {
                Rectangle boudingRect1 = new Rectangle(x - 3, y - 43, psize, psize);
                Rectangle boudingRect2 = new Rectangle(x - 3, y + 57, psize, psize);
                if (boudingRect1.Contains(location)) return connectPoint[0];
                else if (boudingRect2.Contains(location)) return connectPoint[1];
                else return Point.Empty;
            }
            else
            {
                Rectangle boudingRect1 = new Rectangle(x - 43, y - 3, psize, psize);
                Rectangle boudingRect2 = new Rectangle(x + 57, y - 3, psize, psize);
                if (boudingRect1.Contains(location)) return connectPoint[0];
                else if (boudingRect2.Contains(location)) return connectPoint[1];
                else return Point.Empty;
            }
        }
        public override Com clone()
        {
            return new Cap(x + 20, y + 20);
        }
        public override Com stateClone()
        {
            Cap cap = new Cap(x, y);
            cap.name = name;
            cap.value = value;
            return cap;
        }
    }
}
