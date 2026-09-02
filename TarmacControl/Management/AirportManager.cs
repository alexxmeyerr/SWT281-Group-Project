using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarmacControl.Entities;
using TarmacControl.Events;
using TarmacControl.Exeptions;

namespace TarmacControl.Management
{
    internal class AirportManager
    {
        //create lists for the aircrafts and vehicles
        private readonly List<Aircraft> aircraftList = new List<Aircraft>();
        private readonly List<GroundVehicle> vehicleList = new List<GroundVehicle>();

        //creates the events to be used in AirportManager
        public event EventHandler<FlightDelayedEventArgs> FlightDelayed;
        public event EventHandler<TurnaroundCompletedEventArgs> TurnaroundCompleted;
        public event EventHandler<LowFuelEventArgs> LowFuelWarning;

        public void TriggerFlightDelayed(string flightNumber, string reason, int delayMinutes)
        {
            FlightDelayed?.Invoke(this, new FlightDelayedEventArgs(flightNumber, reason, delayMinutes));
        }

        public void TriggerTurnaroundCompleted(string flightNumber)
        {
            TurnaroundCompleted?.Invoke(this, new TurnaroundCompletedEventArgs(flightNumber));
        }

        public void TriggerLowFuelWarning(string aircraftNumber, int fuelLevel)
        {
            LowFuelWarning?.Invoke(this, new LowFuelEventArgs(aircraftNumber, fuelLevel));
        }

        public List<GroundVehicle> GetIdleVehicles()
        {
            return vehicleList.Where(vehicle => vehicle.status == "IsAvailable").ToList();
        }

        public List<Aircraft> GetLowFuelAircraft()
        {
            return aircraftList.Where(aircraft => aircraft.FuelLevel < 20).ToList();
        }


        //create a list of all the gates
        private readonly Dictionary<string, bool> gates = new Dictionary<string, bool>()
        {
            // international and national gates
            { "A1", true }, { "A2", true }, { "A3", true }, { "A4", true }, { "A5", true },
            { "A6", true }, { "A7", true }, { "A8", true }, { "A9", true }, { "A10", true },
            
            // cargo gates
            { "C1", true }, { "C2", true }, { "C3", true }, { "C4", true }, { "C5", true },
            { "C6", true }, { "C7", true }, { "C8", true }, { "C9", true }, { "C10", true },
            
            // private gates
            { "D1", true }, { "D2", true }, { "D3", true }, { "D4", true }, { "D5", true }
        };

        //assign gates to aircrafts
        public void assignGate(string gateName, Aircraft plane)
        {
            if(gates.ContainsKey(gateName) == false)
            {
                throw new ArgumentException($"Gate {gateName} does not exist.");
            }
            if (gateName[0] != plane.AllowedGatePrefix)
            {
                throw new InvalidGateAssignmentException($"{plane.GetType().Name} is not permitted at gate {gateName}");
            }
            if (gates[gateName] == false)
            {
                throw new GateOccupiedException("Gate is occupied");
            }
           
                gates[gateName] = false; 
        }

        public Aircraft FindAircraftByNumber(string aircraftNumbr)
        {
            foreach(var a in aircraftList)
            {
                if(a.AircraftNumber == aircraftNumbr)
                {
                    return a;
                }
            }
            return null;
        }

        //add aircraft to the list
        public void addAircraft(Aircraft plane)
        {
            bool found = false;

            if (plane == null)
            {
                found = true;
                Console.WriteLine("Invalid entry.");
            }
            else
            {
                foreach (var a in aircraftList)
                {
                    if (a.AircraftNumber == plane.AircraftNumber)
                    {
                        found = true;
                        Console.WriteLine("Aircraft number already exists");
                    }
                }
            }

            if(found == false)
            {
                aircraftList.Add(plane);
                Console.WriteLine("Aircraft added successfully.");
                if(plane.FuelLevel < 15)
                {
                    TriggerLowFuelWarning(plane.AircraftNumber, plane.FuelLevel);
                }
            }
        }
        public void viewAllAircrafts()
        {
            if(aircraftList.Count == 0)
            {
                Console.WriteLine("No aircraft listed.");
            }

            foreach (var aircraft in aircraftList)
            {
                Console.WriteLine($"Aircraft Number: {aircraft} \n");
            }
        }

        //add aircraft to the list
        public void addVehicle(GroundVehicle vehicle)
        {
            bool found = false;

            if (vehicle == null)
            {
                found = true;
                Console.WriteLine("Invalid entry");
            }
            else
            {
                foreach (var v in vehicleList)
                {
                    if (v.VehicleID == vehicle.VehicleID)
                    {
                        found = true;
                        Console.WriteLine("Vehicle already exists");
                    }
                }
            }
            if (found == false)
            {
                vehicleList.Add(vehicle);
                Console.WriteLine("Vehicle added successfully.");
            }
        }
        public void viewAllVehicles()
        {
            if (vehicleList.Count == 0)
            {
                Console.WriteLine("No vehicles listed");
            }

            foreach (var v in vehicleList)
            {
                Console.WriteLine($"Vehicle Number: {v} \n");
            }
        }

        public void registerAircraft(Aircraft plane, string gateName)
        {
            try
            {
                assignGate(gateName, plane);
                addAircraft(plane);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not register aircraft: {e.Message}");
            }
        }

        public void registerVehicle(GroundVehicle vehicle, string assignedAircraftNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(assignedAircraftNumber))
                {
                    Aircraft plane = FindAircraftByNumber(assignedAircraftNumber);

                    if (plane == null)
                    {
                        throw new AircraftNotFoundException($"Aircraft {assignedAircraftNumber} does not exist.");
                    }
                }

                addVehicle(vehicle);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not register vehicle: {e.Message}");
            }
        }

        public void DispatchToAircraft(string vehicle, string destination)
        {
            //GroundVehicle groundVehicle;
            bool found = false;

            try
            {
                if (vehicle == null)
                {
                    found = true;
                    Console.WriteLine("Vehicle not found");
                }
                else
                {
                    foreach (var v in vehicleList)
                    {
                        if (v.VehicleID == vehicle)
                        {
                            found = true;
                            if (v.isAvailable == true)
                            {
                                v.Dispatch(destination);
                            }
                            else
                            {
                                throw new ServiceOutOfSequenceException
                                    ($"Vehicle: {vehicle} is currently unavaillable and cannot be dispatched " +
                                    $"until it completes its current task."); 
                            }
                        }
                    }
                }

                if (found == false)
                {
                    Console.WriteLine("Vehicle was not found");
                    // If a vehicle cannot be found to service the aircraft, mark the flight as delayed
                    var plane = FindAircraftByNumber(destination);
                    if (plane != null)
                    {
                        TriggerFlightDelayed(plane.FlightNumber, "No available vehicle to service flight", 15);
                    }
                }
            }
            catch (ServiceOutOfSequenceException ex)
            {
                Console.WriteLine($"Dispatch failed: {ex.Message}");
                // vehicle was busy which caused a delay for the flight
                var plane = FindAircraftByNumber(destination);
                if (plane != null)
                {
                    TriggerFlightDelayed(plane.FlightNumber, ex.Message, 10);
                }
            }
        }

        // Perform a turnaround procedure for an aircraft 
        public void PerformTurnaround(string aircraftNumber)
        {
            var plane = FindAircraftByNumber(aircraftNumber);
            if (plane == null)
            {
                Console.WriteLine($"Aircraft {aircraftNumber} not found.");
                return;
            }

            try
            {
                plane.Turnaround();
                TriggerTurnaroundCompleted(plane.FlightNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Turnaround failed for {aircraftNumber}: {ex.Message}");
            }
        }
        

        public void DisplayOperationalReports()
        {
            List<Aircraft> lowFuelAircraft = GetLowFuelAircraft();
            List<GroundVehicle> idleVehicles = GetIdleVehicles();

            Console.WriteLine();
            Console.WriteLine("===== OPERATIONAL REPORT =====");

            Console.WriteLine();
            Console.WriteLine("Aircraft below 20% fuel");

            if(lowFuelAircraft.Count == 0)
            {
                Console.WriteLine("None");
            } else
            {
                foreach(Aircraft aircraft in lowFuelAircraft)
                {
                    Console.WriteLine($"Flight: {aircraft.AircraftNumber} " + $"Fuel: {aircraft.FuelLevel}%");

                }
            }

            Console.WriteLine();
            Console.WriteLine("Idle ground vehicles: ");

            if(idleVehicles.Count == 0)
            {
                Console.WriteLine("None");
            } else
            {
                foreach(GroundVehicle vehicle in idleVehicles)
                {
                    Console.WriteLine($"Vehicle: {vehicle.VehicleID}" + $"Status: {vehicle.status}");
                }
            }

            Console.WriteLine("=================================");
        }
    }
}
