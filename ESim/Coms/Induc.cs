using ElectronicSimulator.Components;
using System.Collections.Generic;
using System.Drawing;

namespace ElectronicSimulator.Coms
{
    public class Induc : Com
    {
        public Induc(int x, int y)
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

                graphics.DrawString("I" + name, font, brush, new Point(x - 40, y));
                graphics.DrawString(value + "μH", font, brush, new Point(x + 20, y));

                graphics.DrawEllipse(pen, x - 4, y - 43, psize, psize);
                graphics.DrawEllipse(pen, x - 4, y + 57, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y - 43, psize, psize);
                graphics.FillEllipse(fillBrush, x - 4, y + 57, psize, psize);

                Rectangle rect1 = new Rectangle(x - 15, y - 20, 30, 20);
                Rectangle rect2 = new Rectangle(x - 15, y, 30, 20);
                Rectangle rect3 = new Rectangle(x - 15, y + 20, 30, 20);

                graphics.DrawLine(pen, new Point(x, y - 37), new Point(x, y - 20));
                graphics.DrawLine(pen, new Point(x, y + 57), new Point(x, y + 40));
                graphics.DrawArc(pen, rect1, 90, -180);
                graphics.DrawArc(pen, rect2, 90, -180);
                graphics.DrawArc(pen, rect3, 90, -180);
            }
            else
            {
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 50, y - 30, 120, 60));

                graphics.DrawString("I" + name, font, brush, new Point(x, y - 40));
                graphics.DrawString(value + "μH", font, brush, new Point(x, y + 20));

                graphics.DrawEllipse(pen, x - 43, y - 4, psize, psize);
                graphics.DrawEllipse(pen, x + 57, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x - 43, y - 4, psize, psize);
                graphics.FillEllipse(fillBrush, x + 57, y - 4, psize, psize);

                Rectangle rect1 = new Rectangle(x - 20, y - 15, 20, 30);
                Rectangle rect2 = new Rectangle(x, y - 15, 20, 30);
                Rectangle rect3 = new Rectangle(x + 20, y - 15, 20, 30);

                graphics.DrawLine(pen, new Point(x - 37, y), new Point(x - 20, y));
                graphics.DrawLine(pen, new Point(x + 57, y), new Point(x + 40, y));
                graphics.DrawArc(pen, rect1, 0, -180);
                graphics.DrawArc(pen, rect2, 0, -180);
                graphics.DrawArc(pen, rect3, 0, -180);
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
            return new Induc(x + 20, y + 20);
        }
        public override Com stateClone()
        {
            Induc induc = new Induc(x, y);
            induc.name = name;
            induc.value = value;
            return induc;
        }
    }
}
