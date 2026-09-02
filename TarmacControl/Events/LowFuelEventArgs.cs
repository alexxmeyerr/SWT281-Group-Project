using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Events
{
    public class LowFuelEventArgs:System.EventArgs
    {
        public string AircraftNumber { get; }
        public int FuelLevel { get; }

        public LowFuelEventArgs(string aircraftNumber, int fuelLevel)
        {
            this.AircraftNumber = aircraftNumber;
            this.FuelLevel = fuelLevel;
        }
    }
}
