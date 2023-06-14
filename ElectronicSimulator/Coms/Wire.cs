using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicSimulator.Components
{
    public class Wire : Com
    {
        public bool IsSelected { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Wire(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            IsSelected = false;
        }

        public override void Draw(Graphics graphics)
        {
            if (IsSelected)
            {
                graphics.DrawLine(Pens.Red, StartPoint, EndPoint);
            }
            else
            {
                graphics.DrawLine(pen, StartPoint, EndPoint);
            }
        }

        public override bool Contains(Point location)
        {
            double distance = DistanceToLine(location, StartPoint, EndPoint);
            return distance <= pen.Width;
        }
        private double DistanceToLine(Point point, Point lineStart, Point lineEnd)
        {
            double px = point.X;
            double py = point.Y;
            double x1 = lineStart.X;
            double y1 = lineStart.Y;
            double x2 = lineEnd.X;
            double y2 = lineEnd.Y;

            double dx = x2 - x1;
            double dy = y2 - y1;

            double length = Math.Sqrt(dx * dx + dy * dy);

            double distance = Math.Abs((py - y1) * dx - (px - x1) * dy) / length;

            return distance;
        }

        public override bool Connec(Point location)
        {
            return false;
        }

        public override Com Clone()
        {
            return new Wire(StartPoint, EndPoint);
        }

        public override void Rotate(int angle)
        {
            throw new NotImplementedException();
        }
    }
}
