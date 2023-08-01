using ElectronicSimulator.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicSimulator.Coms
{
    public class Lamp : Com
    {
        public bool isLight = false;
        public Lamp(int x, int y)
        {
            connectPoint = new List<Point>();
            connectWire = new List<Wire>();
            this.x = x; this.y = y;
            value = 10;
            connectPoint.Add(new Point(x - 40, y));
            connectPoint.Add(new Point(x + 40, y));
        }

        public override void draw(Graphics graphics)
        {
            if (isRotate)
            {
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 50, y - 50, 60, 100));
                if (isLight)
                    graphics.FillEllipse(new SolidBrush(Color.FromArgb(255, 255, 112)), x - 42, y - 18, 36, 36);
                graphics.DrawString("L" + name, font, brush, new Point(x + 20, y - 10));

                graphics.DrawEllipse(pen, x - 4, y - 43, psize, psize);
                graphics.DrawEllipse(pen, x - 4, y + 37, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y - 43, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y + 37, psize, psize);

                graphics.DrawLine(pen, new Point(x, y - 37), new Point(x, y + 37));
                graphics.DrawEllipse(pen, x - 42, y - 18, 36, 36);
                graphics.DrawRectangle(pen, new Rectangle(x - 6, y - 6, 6, 12));
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(112, 112, 112)), new Rectangle(x - 6, y - 6,  6, 12));
                graphics.DrawLine(pen, new Point(x - 6, y - 6), new Point(x - 20, y - 12));
                graphics.DrawLine(pen, new Point(x - 6, y + 6), new Point(x - 20, y + 12));
                graphics.DrawLine(pen, new Point(x - 20, y - 12), new Point(x - 25, y - 12));
                graphics.DrawLine(pen, new Point(x - 20, y + 12), new Point(x - 25, y + 12));

                PointF[] points = {
                        new Point(x - 25, y - 12), new Point(x - 28, y - 8),
                        new Point(x - 22, y - 4), new Point(x - 28, y),
                        new Point(x - 22, y + 4), new Point(x - 28, y + 8),
                        new Point(x - 25, y + 12)
                        };
                graphics.DrawLines(new Pen(Color.Black, 2), points);
            }
            else
            {
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 50, y - 50, 100, 60));
                if (isLight)
                    graphics.FillEllipse(new SolidBrush(Color.FromArgb(255, 255, 112)), x - 18, y - 42, 36, 36);
                graphics.DrawString("L" + name, font, brush, new Point(x - 10, y + 20));

                graphics.DrawEllipse(pen, x - 43, y - 4, psize, psize);
                graphics.DrawEllipse(pen, x + 37, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x - 43, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x + 37, y - 4, psize, psize);

                graphics.DrawLine(pen, new Point(x - 37, y), new Point(x + 37, y));
                graphics.DrawEllipse(pen, x - 18, y - 42, 36, 36);
                graphics.DrawRectangle(pen, new Rectangle(x - 6, y - 6, 12, 6));
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(112, 112, 112)), new Rectangle(x - 6, y - 6, 12, 6));
                graphics.DrawLine(pen, new Point(x - 6, y - 6), new Point(x - 12, y - 20));
                graphics.DrawLine(pen, new Point(x + 6, y - 6), new Point(x + 12, y - 20));
                graphics.DrawLine(pen, new Point(x - 12, y - 20), new Point(x - 12, y - 25));
                graphics.DrawLine(pen, new Point(x + 12, y - 20), new Point(x + 12, y - 25));

                PointF[] points = { 
                        new Point(x - 12, y - 25), new Point(x - 8, y - 28),
                        new Point(x - 4, y - 22), new Point(x, y - 28),
                        new Point(x + 4, y - 22), new Point(x + 8, y - 28),
                        new Point(x + 12, y - 25)
                        };
                graphics.DrawLines(new Pen(Color.Black, 2), points);
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
                Rectangle boundingRect = new Rectangle(x - 50, y - 50, 60, 100);
                return boundingRect.Contains(location);
            }
            else
            {
                Rectangle boundingRect = new Rectangle(x - 50, y - 50, 100, 60);
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
            return new Lamp(x + 20, y + 20);
        }
        public override Com stateClone()
        {
            Lamp lamp = new Lamp(x, y);
            lamp.name = name;
            lamp.value = value;
            return lamp;
        }
    }
}
