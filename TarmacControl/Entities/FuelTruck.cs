using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarmacControl.Exeptions;

namespace TarmacControl.Entities
{
    internal class FuelTruck : GroundVehicle
    {
        private int incFuelLevel;


        public FuelTruck(string vehicleID, string status, string assignedAircraft, int incFuelLevel) : base(vehicleID, status, assignedAircraft)
        {
            this.incFuelLevel = incFuelLevel;
        }
        public override void PerformService()
        {
            if(incFuelLevel < 25)
            {
                throw new InsufficientFuelException($"Fuel truck {VehicleID} only carries {incFuelLevel}L, which is below the 25L minimum required to service the aircraft.");
            }
            Console.WriteLine("Increasing fuel level...");
        }
    
    }
}
