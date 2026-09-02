using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Entities
{
    internal class CargoPlane: Aircraft
    {
        private double loadWeight;

        public CargoPlane(string AircraftNumber,string flightNumber, string status, string assignedGate, int fuelLevel, double loadWeight) : base(AircraftNumber,flightNumber, status, assignedGate, fuelLevel)
        {
            this.loadWeight = loadWeight;
        }
        public override char AllowedGatePrefix => 'C';

        public override void Turnaround()
        {
            Console.WriteLine("Loading the cargo");
            Console.WriteLine("Fueling the plane");
        }

        public override string ToString()
        {
            return base.ToString() + $"Load Weight: {loadWeight}kg\n";
        }
    }
}
