using ElectronicSimulator.Coms;
using ElectronicSimulator.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicSimulator
{
    public class CircuitState
    {
        public List<Com> Components { get; set; }
        public List<Wire> Wires { get; set; }

        public CircuitState(List<Com> components, List<Wire> wires)
        {
            Components = new List<Com>(components.Select(com => (Com)com.stateClone()));
            Wires = new List<Wire>(wires.Select(wire => (Wire)wire.stateClone()));
        }
    }
}
