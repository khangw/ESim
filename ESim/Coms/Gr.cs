using ElectronicSimulator.Coms;
using System.Collections.Generic;
using System.Drawing;

namespace ElectronicSimulator.Components
{
    public class Gr : Com
    {
        public Gr(int x, int y) 
        {
            connectPoint = new List<Point>();
            connectWire = new List<Wire>();
            this.x = x; this.y = y;
            value = 0;
            connectPoint.Add(new Point(x, y - 20));
        }
        public override void draw(Graphics graphics)
        {
            if (isSelected)
                graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 30, y - 30, 60, 60));
            if (isRotate)
            {
                graphics.FillEllipse(fillBrush, x - 23, y - 3, psize, psize);
                graphics.DrawEllipse(pen, x - 23, y - 3, psize, psize);
                graphics.DrawLine(pen, new Point(x - 17, y), new Point(x, y));
                graphics.DrawLine(pen, new Point(x, y - 20), new Point(x, y + 20));
                graphics.DrawLine(pen, new Point(x + 10, y - 13), new Point(x + 10, y + 13));
                graphics.DrawLine(pen, new Point(x + 20, y - 6), new Point(x + 20, y + 6));
            }
            else
            {
                graphics.FillEllipse(fillBrush, x - 3, y - 23, psize, psize);
                graphics.DrawEllipse(pen, x - 3, y - 23, psize, psize);
                graphics.DrawLine(pen, new Point(x, y - 17), new Point(x, y));
                graphics.DrawLine(pen, new Point(x - 20, y), new Point(x + 20, y));
                graphics.DrawLine(pen, new Point(x - 13, y + 10), new Point(x + 13, y + 10));
                graphics.DrawLine(pen, new Point(x - 6, y + 20), new Point(x + 6, y + 20));
            }
        }
        public override void updateConnectPoints()
        {
            if (isRotate)
            {
                connectPoint[0] = new Point(x - 20, y);
            }
            else
            {
                connectPoint[0] = new Point(x, y - 20);
            }
        }
        public override bool contains(Point location)
        {
            Rectangle boundingRect = new Rectangle(x - 30, y - 30, 60, 60);
            return boundingRect.Contains(location);
        }
        public override Point connects(Point location)
        {
            if (isRotate)
            {
                Rectangle boudingRect = new Rectangle(x - 23, y - 3, psize, psize);
                if (boudingRect.Contains(location))
                    return connectPoint[0];
                else return Point.Empty;
            }
            else
            {
                Rectangle boudingRect = new Rectangle(x - 3, y - 23, psize, psize);
                if (boudingRect.Contains(location))
                    return connectPoint[0];
                else return Point.Empty;
            }
        }
        public override Com clone()
        {
            return new Gr(x + 20, y + 20);
        }
        public override Com stateClone()
        {
            Gr gr = new Gr(x, y);
            gr.name = name;
            gr.value = value;
            return gr;
        }
    }
}
