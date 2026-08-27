using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Entities
{
    internal class PushbackTug: GroundVehicle
    {
        public PushbackTug(string vehicleID, string status, string assignedAircraft):base(vehicleID, status, assignedAircraft)
        {

        }
        public override void PerformService()
        {
            Console.WriteLine("Aircraft being pushed back");
        }
    }
}
