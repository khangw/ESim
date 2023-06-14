using ElectronicSimulator.Components;
using SpiceSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpiceSharp.Components;
using SpiceSharp.Simulations;

namespace ElectronicSimulator
{
    public partial class ESim : Form
    {
        private Circuit ckt = new Circuit();
        private List<Com> coms = new List<Com>();
        private Com copycoms = null;
        private ComponentType currentCom = ComponentType.None;
        private Com selectedcom;
        private int x, y;
        private bool isDoubleClick = false;
        private bool isDrawingWire = false;
        private List<Wire> wires = new List<Wire>();
        private Wire currentWire;
        public enum ComponentType
        {
            None,
            PowerDC1,
            Resistor,
            Grouding
        }
        public ESim()
        {
            InitializeComponent();
        }
        private void Caculated()
        {
            Com powerDC1 = coms.FirstOrDefault(com => com is Vol);
            if (powerDC1 != null)
            {
                Com cur = powerDC1;
                var vol0 = new VoltageSource("V0", "1", "0", powerDC1.value);
                int v = 1, r = 0, g = 0, next = 2, pre = 1;
                ckt.Add(vol0);
                foreach (Wire wire in wires)
                {
                    if (cur.Connec(wire.StartPoint))
                    {
                        foreach (Com com in coms)
                        {
                            if (com == powerDC1) break;
                            if (com.Contains(wire.EndPoint))
                            {
                                cur = com;
                                if (cur is Vol)
                                {
                                    var vol = new VoltageSource("V" + v++, "" + pre++, "" + next++, cur.value);
                                    ckt.Add(vol);
                                }
                                if (cur is Res)
                                {
                                    var res = new Resistor("R" + r++, "" + pre++, "" + next++, cur.value);
                                    ckt.Add(res);
                                }
                                if (cur is Gr)
                                {
                                    var gr = new VoltageSource("Gr", "3", "0", 0.0);
                                    ckt.Add(gr);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
                var dc = new DC("dc", "V0", 0.0, 5.0, 0.001);
                dc.ExportSimulationData += (sender, exportDataEventArgs) =>
                {
                    Console.WriteLine(exportDataEventArgs.GetVoltage("1"));
                };

                // Run the simulation
                dc.Run(ckt);
            }
            else
            {
                Console.WriteLine("No circuit");
            }
        }
        private void Voltage_Click(object sender, EventArgs e)
        {
            currentCom = ComponentType.PowerDC1;
            isDrawingWire = false;
        }
        private void Resistor_Click(object sender, EventArgs e)
        {
            currentCom = ComponentType.Resistor;
            isDrawingWire = false;
        }
        private void GroudingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCom = ComponentType.Grouding;
            isDrawingWire = false;
        }
        private void WireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isDrawingWire = true;
        }
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDrawingWire)
            {
                if (currentWire == null)
                {
                    currentWire = new Wire(System.Drawing.Point.Empty, System.Drawing.Point.Empty);
                }
                if (coms != null)
                {
                    bool startPointEmpty = currentWire.StartPoint == System.Drawing.Point.Empty;
                    bool endPointEmpty = currentWire.EndPoint == System.Drawing.Point.Empty;
                    foreach (Com com in coms)
                    {
                        if (startPointEmpty && com.Connec(e.Location))
                        {
                            currentWire.StartPoint = e.Location;
                            startPointEmpty = false;
                        }
                        else if (endPointEmpty && com.Connec(e.Location))
                        {
                            currentWire.EndPoint = e.Location;
                            endPointEmpty = false;
                            wires.Add(currentWire);
                            Console.WriteLine("Added wire from (" + currentWire.StartPoint.X + ", " + currentWire.StartPoint.Y + ") " +
                                "to (" + currentWire.EndPoint.X + ", " + currentWire.EndPoint.Y + ")");
                                                        
                            currentWire = null;
                        }
                        pictureBox.Refresh();
                    }
                }
            }
            if (e.Clicks == 2)
            {
                isDoubleClick = true;
            }
            else
            {
                isDoubleClick = false;
            }
            System.Drawing.Point clientPoint = pictureBox.PointToClient(e.Location);
            x = clientPoint.X;
            y = clientPoint.Y;
            if (currentCom != ComponentType.None)
            {
                switch (currentCom)
                {
                    case ComponentType.PowerDC1:
                        coms.Add(new Vol(x, y));
                        Console.WriteLine("Added PowerDC1 at (" + x + ", " + y + ")");
                        break;
                    case ComponentType.Resistor:
                        coms.Add(new Components.Res(x, y));
                        Console.WriteLine("Added Resistor at (" + x + ", " + y + ")");
                        break;
                    case ComponentType.Grouding:
                        coms.Add(new Gr(x, y));
                        Console.WriteLine("Added Grouding at (" + x + ", " + y + ")");
                        break;
                }
            }
            else
            {
                foreach (Com com in coms)
                {
                    if (com.Contains(e.Location))
                    {
                        selectedcom = com;
                        break;
                    }
                }
            }
            pictureBox.Refresh();
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedcom != null && !isDoubleClick)
            {
                selectedcom.x = e.X;
                selectedcom.y = e.Y;
                Console.WriteLine("Move " + selectedcom + " to (" + e.X + ", " + e.Y + ")");
                pictureBox.Refresh();
            }
        }
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDoubleClick)
                selectedcom = null;
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedcom != null)
            {
                coms.Remove(selectedcom);
                selectedcom = null;
                pictureBox.Refresh();
            }
            wires.RemoveAll(wire => wire.IsSelected);
            pictureBox.Refresh();
        }

        private void FileToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ShowDropDown();
        }
        private void EditToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            editToolStripMenuItem.ShowDropDown();
        }

        private void CreateToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            createToolStripMenuItem.ShowDropDown();
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (Wire wire in wires)
            {
                if (wire.Contains(e.Location))
                {
                    wire.IsSelected = !wire.IsSelected; // Đảo trạng thái chọn
                    pictureBox.Refresh();
                    break;
                }
            }
        }

        private void PictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (Com com in coms)
            {
                if (com.Contains(e.Location))
                {
                    selectedcom = com;
                    pictureBox.Refresh();
                    break;
                }
            }
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedcom != null)
            {
                copycoms = selectedcom;
            }
        }

        private void PatseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (copycoms != null)
                coms.Add(copycoms.Clone());
                pictureBox.Refresh();
        }

        private void RotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedcom != null)
            {
                selectedcom.Rotate(90); // Xoay hình 90 độ theo chiều kim đồng hồ
                pictureBox.Refresh();
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Caculated();
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            foreach (Com com in coms)
            {
                com.Draw(e.Graphics);
                currentCom = ComponentType.None;
            }
            foreach (Wire wire in wires)
            {
                wire.Draw(e.Graphics);
            }
        }

        
    }
}
