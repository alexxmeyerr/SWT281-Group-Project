using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarmacControl.Interfaces;

namespace TarmacControl.Entities
{
    abstract class GroundVehicle: IDispatchable, IMonitorable
    {
        public string VehicleID { get; protected set; }
        public string status { get; protected set; }
        protected string assignedAircraft;

        public GroundVehicle(string vehicleID, string status, string assignedAircraft)
        {
            this.VehicleID = vehicleID;
            this.status = status;
            this.assignedAircraft = assignedAircraft;
        }

        public abstract void PerformService();

        //handling interfaces
        public bool isAvailable { get { return status == "IsAvailable"; } }

        public void Dispatch(string destination)
        {
            assignedAircraft = destination;
            status = "Dispatched";
            Console.WriteLine($"{VehicleID} dspatched to {destination}");
        }

        public string GetStatusReport()
        {
            return $"{VehicleID}\n=====================\n" +
                $"Status: {status}\n" +
                $"Assigned Aircraft: {assignedAircraft}";
        }

        //fix displaying problem by overiding ToString()
        public override string ToString()
        {
            return $"{VehicleID}\n" +
                $"Status: {status}\n" +
                $"Assigned Aircraft: {assignedAircraft}\n";
        }
    }
}
