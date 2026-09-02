using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Entities
{
    internal class PrivateJet: Aircraft
    {
        private double flightDuration;
        private int passengercount;
        //private double fuel;

        public PrivateJet(string AircraftNumber, string flightNumber, string status, string assignedGate, int fuelLevel, double flightDuration, int passengercount) : base(AircraftNumber,flightNumber, status, assignedGate, fuelLevel)
        {
            this.flightDuration = flightDuration;
            this.passengercount = passengercount;
            //this.fuel = fuel;
        }
        public override char AllowedGatePrefix => 'D';

        public override void Turnaround()
        {
            Console.WriteLine("Fueling");
            Console.WriteLine("Boarding");
        }

        public override string ToString()
        {
            return base.ToString() + $"Passenger count: {passengercount}";
        }
    }
}
