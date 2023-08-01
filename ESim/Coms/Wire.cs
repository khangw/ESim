using System;
using System.Drawing;

namespace ElectronicSimulator.Components
{
    public class Wire
    {
        public Com startComponent;
        public Com endComponent;
        public bool isSelected;
        public string name = "-1";
        public static int penWidth = 2;
        public Pen pen = new Pen(Color.FromArgb(112, 112, 112), penWidth);
        public Pen penselect = new Pen(Color.FromArgb(112, 112, 112), 2*penWidth);
        public Font font = new Font("Arial", 12);
        public Brush brush = Brushes.Gray;
        public Point startPoint { get; set; }
        public Point endPoint { get; set; }

        public Wire(Point startPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public void draw(Graphics graphics)
        {
            graphics.DrawString(name, font, brush, startPoint);
            if (!startComponent.isRotate && !endComponent.isRotate)
            {
                Point middlePoint1 = new Point((startPoint.X + endPoint.X)/2/20*20, startPoint.Y);
                Point middlePoint2 = new Point((startPoint.X + endPoint.X)/2/20*20, endPoint.Y);
                graphics.DrawLine(pen, middlePoint1, startPoint);
                graphics.DrawLine(pen, middlePoint1, middlePoint2);
                graphics.DrawLine(pen, middlePoint2, endPoint);
            }
            else if (!startComponent.isRotate && endComponent.isRotate)
            {
                Point middlePoint = new Point(endPoint.X, startPoint.Y);
                graphics.DrawLine(pen, middlePoint, startPoint);
                graphics.DrawLine(pen, middlePoint, endPoint);
            }
            else if (startComponent.isRotate && !endComponent.isRotate)
            {
                Point middlePoint = new Point(startPoint.X, endPoint.Y);
                graphics.DrawLine(pen, middlePoint, startPoint);
                graphics.DrawLine(pen, middlePoint, endPoint);
            }
            else if (startComponent.isRotate && endComponent.isRotate)
            {
                Point middlePoint;
                if (startPoint.Y <= endPoint.Y)
                {
                    middlePoint = new Point(endPoint.X, startPoint.Y);
                }
                else
                {
                    middlePoint = new Point(startPoint.X, endPoint.Y);
                }
                graphics.DrawLine (pen, middlePoint, startPoint);
                graphics.DrawLine(pen, middlePoint, endPoint);
            }
        }
        public void enterDraw(Graphics graphics)
        {
            graphics.DrawString(name, font, brush, startPoint);
            if (startPoint.X <= endPoint.X && !startComponent.isRotate && !endComponent.isRotate)
            {
                Point middlePoint1 = new Point((startPoint.X + endPoint.X) / 2 / 20 * 20, startPoint.Y);
                Point middlePoint2 = new Point((startPoint.X + endPoint.X) / 2 / 20 * 20, endPoint.Y);
                graphics.DrawLine(penselect, middlePoint1, startPoint);
                graphics.DrawLine(penselect, middlePoint1, middlePoint2);
                graphics.DrawLine(penselect, middlePoint2, endPoint);
            }
            else if (!startComponent.isRotate && endComponent.isRotate)
            {
                Point middlePoint = new Point(endPoint.X, startPoint.Y);
                graphics.DrawLine(penselect, middlePoint, startPoint);
                graphics.DrawLine(penselect, middlePoint, endPoint);
            }
            else if (startComponent.isRotate && !endComponent.isRotate)
            {
                Point middlePoint = new Point(startPoint.X, endPoint.Y);
                graphics.DrawLine(penselect, middlePoint, startPoint);
                graphics.DrawLine(penselect, middlePoint, endPoint);
            }
            else if (startPoint.X <= endPoint.X && startComponent.isRotate && endComponent.isRotate)
            {
                Point middlePoint;
                if (startPoint.Y <= endPoint.Y)
                {
                    middlePoint = new Point(endPoint.X, startPoint.Y);
                }
                else
                {
                    middlePoint = new Point(startPoint.X, endPoint.Y);
                }
                graphics.DrawLine(penselect, middlePoint, startPoint);
                graphics.DrawLine(penselect, middlePoint, endPoint);
            }
        }
        public Wire stateClone()
        {
            return new Wire(startPoint, endPoint);
        }

        public bool contains(Point location)
        {
            if (startPoint.X <= endPoint.X && !startComponent.isRotate && !endComponent.isRotate)
            {
                Point middlePoint1 = new Point((startPoint.X + endPoint.X) / 2, startPoint.Y);
                Point middlePoint2 = new Point((startPoint.X + endPoint.X) / 2, endPoint.Y);
                return (distanceToLine(location, middlePoint1, startPoint) < 4 ||
                    distanceToLine(location, middlePoint1, middlePoint2) < 4 ||
                    distanceToLine(location, middlePoint2, endPoint) < 4) &&
                    between(location.X, startPoint.X, endPoint.X) &&
                    between(location.Y, endPoint.Y, startPoint.Y);
            }
            else if (!startComponent.isRotate && endComponent.isRotate)
            {
                Point middlePoint = new Point(endPoint.X, startPoint.Y);
                return (distanceToLine(location, middlePoint, startPoint) < 4 ||
                    distanceToLine(location, middlePoint, endPoint) < 4) &&
                    between(location.X, middlePoint.X, endPoint.X) &&
                    between(location.Y, middlePoint.Y, startPoint.Y);
            }
            else if (startComponent.isRotate && !endComponent.isRotate)
            {
                Point middlePoint = new Point(startPoint.X, endPoint.Y);
                return (distanceToLine(location, middlePoint, startPoint) < 4 ||
                    distanceToLine(location, middlePoint, endPoint) < 4) &&
                    between(location.X, middlePoint.X, startPoint.X) &&
                    between(location.Y, middlePoint.Y, endPoint.Y);
            }
            double distance = distanceToLine(location, startPoint, endPoint);
            return (distance <= 2);
        }
        private bool between(double i,double x, double y)
        {
            if (x < y)
            {
                return x < i && i < y;
            }
            else
            { 
                return x > i && i > y;
            }
        }
        private double distanceToLine(Point point, Point lineStart, Point lineEnd)
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
    }
}
