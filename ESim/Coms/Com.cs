using ElectronicSimulator.Components;
using System.Collections.Generic;
using System.Drawing;

namespace ElectronicSimulator
{
    public abstract class Com
    {
        public int x, y;
        public double value;
        public bool isSelected;
        public bool isRotate = false;
        public string name;
        public int width = 100;
        public int height = 100;
        public List<Wire> connectWire;
        public List<Point> connectPoint;
        public Font font = new Font("Arial", 12);
        public Brush brush = Brushes.Gray;
        public int psize = 6;
        public static int penWidth = 2;
        public Pen pen = new Pen(Color.FromArgb(112, 112, 112), penWidth);
        public Pen penselect = new Pen(Color.FromArgb(244, 115, 115), penWidth);
        public SolidBrush fillBrush = new SolidBrush(Color.White);
        public SolidBrush fillBrushConnected = new SolidBrush(Color.Yellow);
        public SolidBrush fillBrushSelected = new SolidBrush(Color.FromArgb(225, 225, 225));
        public abstract void draw(Graphics graphics);
        public abstract bool contains(Point location);
        public abstract Point connects(Point location);
        public abstract Com clone();
        public abstract void updateConnectPoints();
        public abstract Com stateClone();
        public string getDrawingInfo()
        {
            string res = "";
            res += "x: " + x + "|";
            res += "y: " + y + "|";
            res += "value: " + value + "|";
            res += "isSelected: " + (isSelected ? "true" : "false") + "|";
            res += "WireList: ";
            foreach(Wire wire in connectWire)
            {
                res += "(" + wire.startPoint + " to " + wire.endPoint + "): ";
                res += wire.startComponent + " and " + wire.endComponent + "; ";
            }
            res += "|ConnectPoints: ";
            foreach (Point p in connectPoint)
            {
                res += p + "; ";
            }
            return res;
        }
    }
}
