using ElectronicSimulator.Components;
using SpiceSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using DPoint = System.Drawing.Point;
using System.IO;
using System.Drawing;
using ElectronicSimulator.Coms;
using System.Xml.Linq;

namespace ElectronicSimulator
{
    public partial class ESim : Form
    {
        private readonly Circuit circuit = new Circuit();
        private readonly List<Com> coms = new List<Com>();
        private Com copyComs = null;
        private Com selectedCom;
        private bool isDrawingWire = false;
        private readonly List<Wire> wires = new List<Wire>();
        private Wire currentWire, enterWire;
        private bool isGrid = true;
        private DPoint enterPoint, enterWirePoint;
        private readonly List<int> vol_num = new List<int>();
        private readonly List<int> res_num = new List<int>();
        private readonly List<int> node_num = new List<int>();
        private readonly List<int> cap_num = new List<int>();
        private readonly List<int> induc_num = new List<int>();
        private readonly List<int> wire_num = new List<int>();
        private readonly List<int> switch_num = new List<int>();
        private readonly List<int> lamp_num = new List<int>();
        private Stack<CircuitState> undoStack = new Stack<CircuitState>();
        private Stack<CircuitState> redoStack = new Stack<CircuitState>();

        public ESim()
        {
            InitializeComponent();
        }
        private void calculated()
        {
            Com power = coms.FirstOrDefault(com => com is DCVol);
            Com output = coms.FirstOrDefault(com => com.name == "out");
            int countLamp = coms.Count(com => com is Lamp) + 1;
            double[] vLamp = new double[countLamp];
            string[] nodeLamp = new string[countLamp];
            if (power != null )
            {
                Wire w1 = power.connectWire[0];
                Wire w2 = power.connectWire[1];
                DPoint anot = power.connectPoint[1];
                if (anot == w1.startPoint || anot == w1.endPoint)
                {
                    w1.name = "0";
                }
                else
                {
                    w2.name = "0";
                }

                foreach (Com com in coms)
                {
                    if (com is Node && com.name != "output") continue;
                    if (com.name == "output")
                    {
                        foreach (Wire w in com.connectWire)
                        {
                            w.name = "out";
                        }
                    }
                    List<string> node = new List<string>();
                    for (int i = 0; i < com.connectWire.Count; i++)
                    {
                        node.Add(com.connectWire[i].name);
                    }
                    if (node.Count == 1) node.Add("0");//Grounding
                    switch (com)
                    {
                        case DCVol vol:
                            var voltage = new VoltageSource("U" + com.name, node[0], node[1], com.value);
                            Console.WriteLine("VoltageSource(U" + com.name + ", " + node[0] + ", " + node[1] + ", " + com.value + ")");
                            circuit.Add(voltage);
                            break;
                        case Gr gr:
                            var grounding = new VoltageSource("G", node[0], node[1], 0);
                            Console.WriteLine("VoltageSource(G, " + node[0] + "," + node[1] + ", 0)");
                            circuit.Add(grounding);
                            break;
                        case Res res:
                            var resistor = new Resistor("R" + com.name, node[0], node[1], com.value * 10e3);
                            Console.WriteLine("Resistor(R" + com.name + ", " + node[0] + ", " + node[1] + ", " + com.value * 10e3 + ")");
                            circuit.Add(resistor);
                            break;
                        case Cap cap:
                            var capacitor = new Capacitor("C" + com.name, node[0], node[1], com.value * 10e-6);
                            Console.WriteLine("Capacitor(C" + com.name + ", " + node[0] + ", " + node[1] + ", " + com.value * 10e-6 + ")");
                            circuit.Add(capacitor);
                            break;
                        case Induc induc:
                            var inductor = new Inductor("I" + com.name, node[0], node[1], com.value * 10e-6);
                            Console.WriteLine("Inductor(I" + com.name + ", " + node[0] + ", " + node[1] + ", " + com.value * 10e-6 + ")");
                            circuit.Add(inductor);
                            break;
                        case Lamp l:
                            var lamp = new Resistor("L" + com.name, node[0], node[1], com.value);
                            Console.WriteLine("Lamp(L" + com.name + ", " + node[0] + ", " + node[1] + ", " + com.value + ")");
                            circuit.Add(lamp);
                            nodeLamp[int.Parse(com.name)] = node[0];
                            break;
                        case Switch switches:
                            var sw = new Resistor("S" + com.name, node[0], node[1], com.value);
                            Console.WriteLine("S(S" + com.name + ", " + node[0] + ", " + node[1] + ", " + com.value + ")");
                            circuit.Add(sw);
                            break;
                        default:
                            break;
                    }

                }

                // Run the simulation
                var dc = new DC("dc", "U" + power.name, -5.0, 5.0, 0.5);
                double v_out = 0.0;
                dc.ExportSimulationData += (sender, exportDataEventArgs) =>
                {
                    v_out = exportDataEventArgs.GetVoltage("out");
                    for (int i = 1; i < countLamp; i++)
                    {
                        vLamp[i] = exportDataEventArgs.GetVoltage(nodeLamp[i]);
                    }
                };
                dc.Run(circuit);
                for (int i = 1; i < countLamp; i++)
                {
                    Com lamp = coms.FirstOrDefault(com => (com is Lamp && com.name == i + ""));
                    if (vLamp[i] > 10e-5)
                        ((Lamp)lamp).isLight = true;
                    else
                        ((Lamp)lamp).isLight = false;
                }
                pictureBox.Refresh();
                RunCircuit runCircuit = new RunCircuit(v_out, power.value);
                DialogResult result = runCircuit.ShowDialog();
                if (result == DialogResult.OK)
                {
                    for (int i = 1; i < countLamp; i++)
                    {
                        Com lamp = coms.FirstOrDefault(com => (com is Lamp && com.name == i + ""));
                        ((Lamp)lamp).isLight = false;
                    }
                    pictureBox.Refresh();
                }
            }
            else
            {
                MessageBox.Show("No power or no output", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Clear circuit and processed components after simulation
            circuit.Clear();
        }
        private void voltage_Click(object sender, EventArgs e)
        {
            DCVol v = new DCVol(100, 100);
            selectedCom = v;
            int i = 0;
            while (vol_num.Contains(i))
            {
                i++;
            }
            v.name = "" + i;
            coms.Add(v);
            vol_num.Add(i);
            pictureBox.Refresh();
        }
        private void resistor_Click(object sender, EventArgs e)
        {
            Res r = new Res(100, 100);
            selectedCom = r;
            int i = 0;
            while (res_num.Contains(i))
            {
                i++;
            }
            r.name = "" + i;
            coms.Add(r);
            res_num.Add(i);
            pictureBox.Refresh();
        }
        private void grounding_Click(object sender, EventArgs e)
        {
            Gr g = new Gr(100, 100);
            selectedCom = g;
            coms.Add(g);
            pictureBox.Refresh();
        }
        private void capacitor_Click(object sender, EventArgs e)
        {
            Cap c = new Cap(100, 100);
            selectedCom = c;
            int i = 0;
            while (cap_num.Contains(i))
            {
                i++;
            }
            c.name = "" + i;
            coms.Add(c);
            cap_num.Add(i);
            pictureBox.Refresh();
        }

        private void inductor_Click(object sender, EventArgs e)
        {
            Induc induc = new Induc(100, 100);
            selectedCom = induc;
            int i = 0;
            while (induc_num.Contains(i))
            {
                i++;
            }
            induc.name = "" + i;
            coms.Add(induc);
            induc_num.Add(i);
            pictureBox.Refresh();
        }
        private void node_Click(object sender, EventArgs e)
        {
            Node node = new Node(100, 100);
            selectedCom = node;
            int i = 0;
            while (node_num.Contains(i))
            {
                i++;
            }
            node.name = "" + i;
            coms.Add(node);
            node_num.Add(i);
            pictureBox.Refresh();
        }
        private void output_Click(object sender, EventArgs e)
        {
            Node node = new Node(100, 100);
            selectedCom = node;
            node.name = "out";
            coms.Add(node);
            pictureBox.Refresh();
        }
        private void switch_Click(object sender, EventArgs e)
        {
            Switch sw = new Switch(100, 100);
            selectedCom = sw;
            int i = 1;
            while (switch_num.Contains(i))
            {
                i++;
            }
            sw.name = "" + i;
            coms.Add(sw);
            switch_num.Add(i);
            pictureBox.Refresh();
        }
        private void lamp_Click(object sender, EventArgs e)
        {
            Lamp lamp = new Lamp(100, 100);
            selectedCom = lamp;
            int i = 1;
            while (lamp_num.Contains(i))
            {
                i++;
            }
            lamp.name = "" + i;
            coms.Add(lamp);
            lamp_num.Add(i);
            pictureBox.Refresh();
        }
        private void wire_Click(object sender, EventArgs e)
        {
            isDrawingWire = !isDrawingWire;
        }
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDrawingWire)
            {
                if (currentWire == null) currentWire = new Wire(DPoint.Empty, DPoint.Empty);
                if (coms == null)
                {
                    MessageBox.Show("It doesn't have any components", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                bool isStartPointEmpty = currentWire.startPoint == DPoint.Empty;
                bool isEndPointEmpty = currentWire.endPoint == DPoint.Empty;
                foreach (Com com in coms)
                {
                    /*if (enterWire != null)
                    {
                        currentWire.startPoint = e.Location;
                        currentWire.name = enterWire.name;
                    }*/
                    if (isStartPointEmpty && com.connects(e.Location) != DPoint.Empty)
                    {
                        currentWire.startComponent = com;
                        com.connectWire.Add(currentWire);
                        currentWire.startPoint = com.connects(e.Location);
                        if (com is DCVol && com.connectPoint[1] == com.connects(e.Location))
                            currentWire.name = "0";
                        if (com is Node)
                        {
                            currentWire.name = com.name;
                            if (com.name != "out") wire_num.Add(int.Parse(com.name));
                        }
                        isStartPointEmpty = false;
                        Console.WriteLine("StartPoint: " + com.connects(e.Location));
                    }
                    else if (isEndPointEmpty && com.connects(e.Location) != DPoint.Empty && com.connects(e.Location) != currentWire.startPoint)
                    {
                        currentWire.endComponent = com;
                        com.connectWire.Add(currentWire);
                        currentWire.endPoint = com.connects(e.Location);

                        if (com is DCVol && com.connectPoint[1] == com.connects(e.Location))
                        {
                            currentWire.name = "0";
                        }
                        if (com is Node)
                        {
                            currentWire.name = com.name;
                            if (com.name != "out") wire_num.Add(int.Parse(com.name));
                        }

                        if (currentWire.name == "-1")
                        {
                            int i = 1;
                            while (wire_num.Contains(i))
                            {
                                i++;
                            }
                            currentWire.name = "" + i;
                            wire_num.Add(i);
                        }

                        wires.Add(currentWire);
                        isEndPointEmpty = false;
                        Console.WriteLine("EndPoint: " + com.connects(e.Location));
                        Console.WriteLine("Added wire from (" + currentWire.startPoint + ") " +
                            "to (" + currentWire.endPoint + ")");

                        currentWire = null;
                        pictureBox.Refresh();
                    }
                }
            }
            foreach (Com com in coms)
            {
                if (com.contains(e.Location) && e.Button == MouseButtons.Left)
                {
                    com.isSelected = true;
                    if (com.isSelected)
                    {
                        coms.ForEach(c =>
                        {
                            if (c.isSelected && c != com)
                            {
                                c.isSelected = false;
                            }
                        });
                    }
                    selectedCom = com;
                }
                else
                {
                    com.isSelected = false;
                }
                if (com.contains(e.Location) && e.Button == MouseButtons.Right)
                {
                    if (com is Switch)
                    {
                        ((Switch)com).isTurnOn = !((Switch)com).isTurnOn;
                        if (((Switch)com).isTurnOn) com.value = 0;
                        else com.value = 10e9;
                        pictureBox.Refresh();
                    }
                    else
                    {
                        ChangeValue valueForm = new ChangeValue();
                        DialogResult result = valueForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            double value;
                            if (double.TryParse(valueForm.TextBoxValue, out value))
                            {
                                com.value = value;
                                pictureBox.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("Value must be a number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        valueForm.Close();
                    }
                }
            }
            foreach (Wire wire in wires)
            {
                if (wire.contains(e.Location) && e.Button == MouseButtons.Left)
                    wire.isSelected = !wire.isSelected;
            }
            SaveCircuitState();
        }
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedCom != null)
            {
                isDrawingWire = false;
                List<DPoint> preMove = new List<DPoint>(selectedCom.connectPoint);
                //Moving component
                selectedCom.x = e.X / 20 * 20;
                selectedCom.y = e.Y / 20 * 20;
                //Console.WriteLine("Move " + selectedCom + " to (" + e + ")");
                selectedCom.updateConnectPoints();

                //Moving wires connected to component
                foreach (Wire wire in wires)
                {
                    if (wire.startComponent == selectedCom || wire.endComponent == selectedCom)
                    {
                        foreach (DPoint point in preMove)
                        {
                            if (point == wire.startPoint)
                            {
                                int index = preMove.IndexOf(point);
                                wire.startPoint = selectedCom.connectPoint[index];
                            }
                            if (point == wire.endPoint)
                            {
                                int index = preMove.IndexOf(point);
                                wire.endPoint = selectedCom.connectPoint[index];
                            }
                        }
                    }
                }
            }
            //Enter connect point of component
            foreach (Com com in coms)
            {
                if (com.contains(e.Location))
                {
                    enterPoint = com.connects(e.Location);
                    if (enterPoint != null)
                        isDrawingWire = true;
                    else
                        isDrawingWire = false;
                }
            }
            //Enter wire for make a new branch of wire
            foreach (Wire wire in wires)
            {
                if (wire.contains(e.Location))
                {
                    enterWire = wire;
                    break;
                }
                else
                {
                    enterWire = null;
                }
            }
            pictureBox.Refresh();
        }
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            selectedCom = null;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Com com in coms)
            {
                for (int i = 0; i < com.connectWire.Count; i++)
                {
                    if (com.connectWire[i].isSelected)
                        com.connectWire.Remove(com.connectWire[i]);
                }
                if (com.name == "out" || com is Gr) continue;
                int c = int.Parse(com.name);
                if (com is DCVol && com.isSelected) vol_num.Remove(c);
                if (com is Res && com.isSelected) res_num.Remove(c);
                if (com is Node && com.isSelected) node_num.Remove(c);
                if (com is Cap && com.isSelected) cap_num.Remove(c);
                if (com is Induc && com.isSelected) induc_num.Remove(c);
                if (com.isSelected)
                {
                    wires.RemoveAll(w => com.connectWire.Contains(w));
                }
            }
            coms.RemoveAll(com => com.isSelected);
            wires.RemoveAll(wire => wire.isSelected);
            pictureBox.Refresh();
        }

        private void fileToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ShowDropDown();
        }
        private void editToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            editToolStripMenuItem.ShowDropDown();
        }
        private void createToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            createToolStripMenuItem.ShowDropDown();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedCom != null)
            {
                copyComs = selectedCom;
            }
        }
        private void patseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (copyComs != null)
                coms.Add(copyComs.clone());
            pictureBox.Refresh();
        }
        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(Com com in coms)
            {
                if (com.isSelected)
                {
                    com.isRotate = !com.isRotate;
                    com.updateConnectPoints();
                }
            }
            pictureBox.Refresh();
            SaveCircuitState();
        }
        private void SaveCircuitState()
        {
            CircuitState currentState = new CircuitState(coms, wires);
            undoStack.Push(currentState);
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                CircuitState previousState = undoStack.Pop();
                redoStack.Push(new CircuitState(coms, wires));

                coms.Clear();
                wires.Clear();

                foreach (Com component in previousState.Components)
                {
                    coms.Add(component.stateClone());
                }

                foreach (Wire wire in previousState.Wires)
                {
                    wires.Add(wire.stateClone());
                }

                pictureBox.Refresh();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                CircuitState nextState = redoStack.Pop();
                undoStack.Push(new CircuitState(coms, wires));

                coms.Clear();
                wires.Clear();

                foreach (Com component in nextState.Components)
                {
                    coms.Add(component.stateClone());
                }

                foreach (Wire wire in nextState.Wires)
                {
                    wires.Add(wire.stateClone());
                }

                pictureBox.Refresh();
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calculated();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int numOfCells = 200;
            int cellSize = 20;
            Pen p = new Pen(Color.FromArgb(225, 225, 225));
            Pen enterpen = new Pen(Color.FromArgb(244, 115, 115), 2);
            SolidBrush fill = new SolidBrush(Color.FromArgb(225, 225, 225));
            SolidBrush enterfill = new SolidBrush(Color.FromArgb(112, 112, 112));
            for (int x = 0; x < numOfCells; x++)
            {
                for (int y = 0; y < numOfCells; y++)
                {
                    g.DrawEllipse(p, x * cellSize - 2, y * cellSize - 2, 4, 4);
                    g.FillEllipse(fill, x * cellSize - 2, y * cellSize - 2, 4, 4);
                }
            }
            if (isGrid)
            {
                for (int y = 0; y < numOfCells; y++)
                {
                    g.DrawLine(p, 0, y*cellSize, numOfCells*cellSize, y*cellSize);
                }

                for (int x = 0; x < numOfCells; x++)
                {
                    g.DrawLine(p, x*cellSize, 0, x*cellSize, numOfCells*cellSize);
                }
            }

            foreach (Com com in coms)
            {
                com.draw(e.Graphics);
            }
            foreach (Wire wire in wires)
            {
                wire.draw(e.Graphics);
            }
            if (enterPoint != DPoint.Empty)
            {
                g.FillEllipse(enterfill, enterPoint.X - 4, enterPoint.Y - 4, 8, 8);
            }
            if (enterWire != null)
            {
                enterWire.enterDraw(e.Graphics);
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = @"C:\Users\user01\OneDrive\Documents";
            saveDialog.Title = "Save Your File Here";
            saveDialog.Filter = "Text files (*.txt)|*.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveDialog.FileName;
                saveDrawings(filePath);
            }
        }
        private void saveDrawings(string filePath)
        {
            List<string> drawingData = new List<string>();
            foreach (Com com in coms)
            {
                string drawingInfo = com.getDrawingInfo();
                drawingData.Add(com + "\n" + drawingInfo + "//");
            }
            File.WriteAllLines(filePath, drawingData);
            MessageBox.Show("Drawings saved successfully.", "Save Drawings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\user01\OneDrive\Documents";
            openFileDialog.Title = "Open Your File";
            openFileDialog.Filter = "Text files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                openDrawings(filePath);
            }
        }
        private void openDrawings(string filePath)
        {
            string[] drawingData = File.ReadAllLines(filePath);
            coms.Clear();
            wires.Clear();
            foreach (string drawingInfo in drawingData)
                createDrawingFromInfo(drawingInfo);
            pictureBox.Refresh();
            MessageBox.Show("Drawings opened successfully.", "Open Drawings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isGrid = !isGrid;
            pictureBox.Refresh();
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            panel.BackColor = Color.LightGray;
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void createDrawingFromInfo(string drawingInfo)
        {
            string[] infoElements = drawingInfo.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string element in infoElements)
            {
                Com component = null;
                string[] properties = element.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(properties[1].Substring(properties[1].IndexOf(": ") + 2));
                int y = int.Parse(properties[2].Substring(properties[2].IndexOf(": ") + 2));
                double value = double.Parse(properties[3].Substring(properties[3].IndexOf(": ") + 2));
                bool isSelected = bool.Parse(properties[4].Substring(properties[4].IndexOf(": ") + 2));

                switch (properties[0])
                {
                    case "ElectronicSimulator.Components.Vol":
                        component = new DCVol(x, y);
                        break;
                    case "ElectronicSimulator.Components.Res":
                        component = new Res(x, y);
                        break;
                    case "ElectronicSimulator.Components.Gr":
                        component = new Gr(x, y);
                        break;
                    case "ElectronicSimulator.Components.Node":
                        component = new Node(x, y);
                        break;
                    default: break;
                }
                component.value = value;
                component.isSelected = isSelected;
                string[] wirelist = properties[5].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < wirelist.Length; i++)
                {
                    int startPointX = int.Parse(wirelist[i].Substring(wirelist[i].IndexOf("X=") + 2, wirelist[i].IndexOf(',') - wirelist[i].IndexOf("X=") - 2));
                    int startPointY = int.Parse(wirelist[i].Substring(wirelist[i].IndexOf("Y=") + 2, wirelist[i].IndexOf('}') - wirelist[i].IndexOf("Y=") - 2));
                    int endPointX = int.Parse(wirelist[i].Substring(wirelist[i].LastIndexOf("X=") + 2, wirelist[i].LastIndexOf(',') - wirelist[i].LastIndexOf("X=") - 2));
                    int endPointY = int.Parse(wirelist[i].Substring(wirelist[i].LastIndexOf("Y=") + 2, wirelist[i].LastIndexOf('}') - wirelist[i].LastIndexOf("Y=") - 2));
                    DPoint startPoint = new DPoint(startPointX, startPointY);
                    DPoint endPoint = new DPoint(endPointX, endPointY);
                    Wire wire = new Wire(startPoint, endPoint);
                    wires.Add(wire);
                }
                string[] pointlist = properties[6].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < pointlist.Length; i++)
                {
                    int pointX = int.Parse(wirelist[i].Substring(wirelist[i].IndexOf("X=") + 2, wirelist[i].IndexOf(',') - wirelist[i].IndexOf("X=") - 2));
                    int pointY = int.Parse(wirelist[i].Substring(wirelist[i].IndexOf("Y=") + 2, wirelist[i].IndexOf('}') - wirelist[i].IndexOf("Y=") - 2));
                    DPoint point = new DPoint(pointX, pointY);
                    component.connectPoint.Add(point);
                }
            }
        }
    }
}