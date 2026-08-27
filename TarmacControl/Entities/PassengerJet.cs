using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Entities
{
    internal class PassengerJet: Aircraft
    {
        private bool boarding;
        private double baggageWeight;

        public PassengerJet(string AircraftNumber,string flightNumber, string status, string assignedGate, int fuelLevel, bool boarding, double baggageWeight): base(AircraftNumber,flightNumber, status, assignedGate, fuelLevel)
        {
            this.boarding = boarding;
            this.baggageWeight = baggageWeight;
        }
        public override char AllowedGatePrefix => 'A';
        public override void Turnaround()
        {
            Console.WriteLine("Baggage");
            Console.WriteLine("Fueling");
            Console.WriteLine("Boarding");
        }

        public override string ToString()
        {
            return base.ToString() + $"Baggage weight: {baggageWeight}kg\n";
        }
    }
}
