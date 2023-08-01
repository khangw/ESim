using ElectronicSimulator.Coms;
using System.Collections.Generic;
using System.Drawing;

namespace ElectronicSimulator.Components
{
    public class Res : Com
    {
        public Res(int x, int y)
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
                PointF[] points = {
                new Point(x, y - 37), new Point(x, y - 20),
                new Point(x - 12, y - 14), new Point(x + 12, y - 6),
                new Point(x - 12, y + 2), new Point(x + 12, y + 10),
                new Point(x - 12, y + 18), new Point(x + 12, y + 26),
                new Point(x - 12, y + 34), new Point(x, y + 40),
                new Point(x, y + 57),
                };
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 30, y - 50, 60, 120));

                graphics.DrawString("R" + name, font, brush, new Point(x - 40, y));
                graphics.DrawString(value + "kΩ", font, brush, new Point(x + 20, y));

                graphics.DrawEllipse(pen, x - 4, y - 43, psize, psize);
                graphics.DrawEllipse(pen, x - 4, y + 57, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y - 43, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y + 57, psize, psize);

                graphics.DrawLines(pen, points);
            }
            else
            {
                PointF[] points = {
                new Point(x - 37,y), new Point(x - 20, y),
                new Point(x - 14, y - 12), new Point(x - 6, y + 12),
                new Point(x + 2, y - 12), new Point(x + 10, y + 12),
                new Point(x + 18, y - 12), new Point(x + 26, y + 12),
                new Point(x + 34, y - 12), new Point(x + 40, y),
                new Point(x + 57, y),
                };

                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 50, y - 30, 120, 60));
                
                graphics.DrawString("R" + name, font, brush, new Point(x, y - 40));
                graphics.DrawString(value + "kΩ", font, brush, new Point(x, y + 20));

                graphics.DrawEllipse(pen, x - 43, y - 4, psize, psize);
                graphics.DrawEllipse(pen, x + 57, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x - 43, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x + 57, y - 4, psize, psize);
                graphics.DrawLines(pen, points);
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
            return new Res(x + 20, y + 20);
        }
        public override Com stateClone()
        {
            Res res = new Res(x, y);
            res.name = name;
            res.value = value;
            return res;
        }
    }
}
