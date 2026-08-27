using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using TarmacControl.Interfaces;

namespace TarmacControl.Entities
{
    abstract class Aircraft: IDispatchable, IMonitorable
    {
        //Encapsolation
        //only this class can acess these properties
        public string AircraftNumber { get; protected set; }
        protected string flightNumber;
        protected string status;
        protected string assignedGate;
        protected int fuelLevel;

        public string FlightNumber { get { return flightNumber; } }

        //create a property that will test and ensure that the aircrafts are assigned to the correct block of gates.
        public abstract char AllowedGatePrefix { get; }
        //create lists for the flight
        private readonly List<Aircraft> flightList = new List<Aircraft>();
        
        public int FuelLevel
        {
            get { return fuelLevel; }
        }

        public Aircraft(string AircraftNumber, string flightNumber, string status, string assignedGate, int fuelLevel)
        {
            this.AircraftNumber = AircraftNumber;
            this.flightNumber = flightNumber;
            this.status = status;
            this.assignedGate = assignedGate;
            

            //ensure that the fuel level is between 0 and 100
            if(fuelLevel >= 0 && fuelLevel <= 100)
            {
                this.fuelLevel = fuelLevel;
            }
            else
            {
                Console.WriteLine("Invalid fuel level");
            }
        }
        

        //add flight to the list
        public void addFlight(Aircraft flight)
        {
            if (flight == null)
            {
                Console.WriteLine("Invalid entry");
            }
            else
            {
                foreach (var f in flightList)
                {
                    if (f.ToString() == flight.ToString())
                    {
                        Console.WriteLine("Flight number already exists");
                    }
                    else
                    {
                        flightList.Add(flight);
                        Console.WriteLine("Flight added successfully.");
                    }
                }
            }
        }

        public abstract void Turnaround();


        //Handling the interfaces
        public bool isAvailable { get { return status == "Arrived"; } }

        public void Dispatch(string destination)
        {
            assignedGate = destination;
            status = "Dispatched";
            Console.WriteLine($"{AircraftNumber} dspatched to {destination}");
        }

        public string GetStatusReport()
        {
            return $"{AircraftNumber}\n=====================\n" +
                $"Status: {status}\n" +
                $"Gate: {assignedGate}\n" +
                $"Fuel: {fuelLevel}%";
        }

        //fix displaying problem by overiding ToString()
        public override string ToString()
        {
            return $"{AircraftNumber}\n" +
                $"Flight: {flightNumber}\n" +
                $"Status: {status}\n" +
                $"Gate: {assignedGate}\n" +
                $"Fuel: {fuelLevel}%\n";
        }
    }
}
