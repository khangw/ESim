using ElectronicSimulator.Coms;
using System.Collections.Generic;
using System.Drawing;

namespace ElectronicSimulator.Components
{
    public class Node : Com
    {
        public Node(int x, int y)
        {
            connectPoint = new List<Point>();
            connectWire = new List<Wire>();
            this.x = x; this.y = y;
            connectPoint.Add(new Point(x, y));
        }

        public override void draw(Graphics graphics)
        {
            graphics.FillEllipse(fillBrush, x - 3, y - 3, psize, psize);

            if (name != null && name.Equals("out"))
            {
                if (isSelected)
                    graphics.FillRectangle(fillBrushSelected, new Rectangle(x - 10, y - 10, 20, 20));

                graphics.DrawEllipse(pen, x - 3, y - 3, psize, psize);
                graphics.DrawLine(pen, new Point(x + 3, y - 3), new Point(x + 13, y - 17));
                graphics.DrawLine(pen, new Point(x + 3, y - 3), new Point(x + 13, y - 14));
                graphics.DrawLine(pen, new Point(x + 3, y - 3), new Point(x + 13, y - 13));
                graphics.DrawLine(pen, new Point(x + 3, y - 3), new Point(x + 14, y - 11));
                graphics.DrawEllipse(pen, x + 10, y - 30, 20, 20);

                graphics.DrawString("V", font, brush, new Point(x + 12, y - 28));
            }
            else
            {
                if (isSelected)
                    graphics.DrawEllipse(penselect, x - 3, y - 3, psize, psize);
                else
                    graphics.DrawEllipse(pen, x - 3, y - 3, psize, psize);
            }
        }
        public override void updateConnectPoints()
        {
            connectPoint[0] = new Point(x, y);
        }

        public override bool contains(Point location)
        {

            Rectangle boundingRect = new Rectangle(x - 3, y - 3, psize, psize);
            return boundingRect.Contains(location);

        }
        public override Point connects(Point location)
        {
            if (contains(location))
                return connectPoint[0];
            else return Point.Empty;
        }
        public override Com clone()
        {
            return new Node(x + 20, y + 20);
        }
        public override Com stateClone()
        {
            Node node = new Node(x, y);
            node.name = name;
            node.value = value;
            return node;
        }
    }
}
