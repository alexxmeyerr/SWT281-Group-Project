using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TarmacControl.Entities;
using TarmacControl.Management;
using TarmacControl.UI;
using TarmacControl.Events;
using System.Net;


namespace TarmacControl.UI
{
    //add enums that will hold the menu values for each different menu
    enum MenuOption
    {
        ViewAllAircraft = 1, //set the first value to have an integer value of 1
        ViewAllVehicles,
        RegisterAircraft,
        RegisterVehicle,
        DispatchVehicle,
        AssignAircraft,
        CompleteTurnaround,
        ViewEventLog,
        ViewOperationalReports,
        Exit
    }

    enum AircraftType
    {
        Passenger = 1,
        Cargo,
        Private
    }

    enum VehicleType
    {
        Fuel = 1,
        Pushback,
        Baggage
    }
    internal class ConsoleMenu
    {
        private readonly AirportManager airportManager;

        public AirportManager AirportManager => airportManager;
        private static readonly List<string> eventLog = new List<string>();
       

        public ConsoleMenu()
        {
            airportManager = new AirportManager();
        }
        // returns false when the program should exit
        public bool ManageSelection(MenuOption input)
        {
            switch (input)
            {
                case MenuOption.ViewAllAircraft:
                    airportManager.viewAllAircrafts();
                    break;
                case MenuOption.ViewAllVehicles:
                    airportManager.viewAllVehicles();
                    break;
                case MenuOption.RegisterAircraft:
                    RegisterAircraft();
                    break;
                case MenuOption.RegisterVehicle:
                    RegisterVehicle();
                    break;
                case MenuOption.DispatchVehicle:
                    DispatchVehicle();
                    break;
                case MenuOption.AssignAircraft:
                    AssignAircraft();
                    break;
                case MenuOption.CompleteTurnaround:
                    CompleteTurnaround();
                    break;
                case MenuOption.ViewEventLog:
                    ViewEventLog();
                    break;
                case MenuOption.ViewOperationalReports:
                    airportManager.DisplayOperationalReports();
                    break;
                case MenuOption.Exit:
                    return false;
                default:
                    Console.WriteLine("The option that you have selected is not valid");
                    break;
            }
            return true;
        }

        public void RegisterAircraft()
        {//ask for all respective info to register an aircraft
            try
            {
                Console.WriteLine("=========REGISTERING AIRCRAFT===============");
                Console.Write("Airplane number:");
                string airplaneNumber = (Console.ReadLine());

                Console.Write("Flight number:");
                string flightNumber = (Console.ReadLine());

                Console.Write("Gate assigned:");
                string assignedGate = (Console.ReadLine());

                Console.Write("Fuel level (%):");
                int fuelLevel = int.Parse(Console.ReadLine());

                Console.WriteLine("Select an aircraft type:");

                foreach (AircraftType t in Enum.GetValues(typeof(AircraftType)))
                {
                    Console.WriteLine($"{(int)t}. {t}");
                }
            
                int type = int.Parse(Console.ReadLine());

                AircraftType selectedType = (AircraftType)type;

                Aircraft newAircraft;

                switch (selectedType)
                {
                    case AircraftType.Passenger:
                        Console.Write("Baggage weight:");
                        double baggage = double.Parse(Console.ReadLine());
                        newAircraft = new PassengerJet(airplaneNumber, flightNumber, "Arriving", assignedGate, fuelLevel, false, baggage);
                        break;
                    case AircraftType.Cargo:
                        Console.Write("Load weight:");
                        double load = double.Parse(Console.ReadLine());
                        newAircraft = new CargoPlane(airplaneNumber, flightNumber, "Arriving", assignedGate, fuelLevel, load);
                        break;
                    case AircraftType.Private:
                        Console.Write("Flight duration (hours): ");
                        double flightDuration = double.Parse(Console.ReadLine());
                        Console.Write("Passenger count:");
                        int passengers = int.Parse(Console.ReadLine());
                        newAircraft = new PrivateJet(airplaneNumber, flightNumber, "Arriving", assignedGate, fuelLevel, flightDuration, passengers);
                        break;
                    default:
                        Console.WriteLine("Invalid aircraft type");
                        return;
                }
                airportManager.registerAircraft(newAircraft, assignedGate);
            }
            catch(FormatException)
            {
                Console.WriteLine("Invalid Entry. Please try again");
            }
            
            
        }

        public void RegisterVehicle()
        {
         //ask for all the information to register a vehicle
            try
            {
                Console.WriteLine("=======================REGISTERING A VEHICLE===========================");
                Console.Write("Vehicle ID:");
                string vehicleId = Console.ReadLine();

                Console.Write("Status:");
                string status = Console.ReadLine();

                Console.Write("Assigned aircraft (or leave blank):");
                string assignedAircraft = Console.ReadLine();

                Console.WriteLine("Select a vehicle type:");
                foreach (VehicleType t in Enum.GetValues(typeof(VehicleType)))
                {
                    Console.WriteLine($"{(int)t}. {t}");
                }

                int type = int.Parse(Console.ReadLine());
                VehicleType selectedType = (VehicleType)type;

                GroundVehicle newVehicle;

                switch (selectedType)
                {
                    case VehicleType.Fuel:
                        Console.WriteLine("To what level does the fuel need to be increased?");
                        int fuelinc = int.Parse(Console.ReadLine());
                        newVehicle = new FuelTruck(vehicleId, status, assignedAircraft, fuelinc);
                        break;
                    case VehicleType.Pushback:
                        newVehicle = new PushbackTug(vehicleId, status, assignedAircraft);
                        break;
                    case VehicleType.Baggage:
                        newVehicle = new BaggageCart(vehicleId, status, assignedAircraft);
                        break;
                    default:
                        Console.WriteLine("Invalid vehicle type");
                        return;
                }

                airportManager.addVehicle(newVehicle);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid entry. Please try again.");
            }
        }
        
        public void DispatchVehicle()
        {
            Console.WriteLine("=========VEHICLE DISPATCH=========================");
            Console.Write("Enter vehicle ID:");
            string vehicle = Console.ReadLine();

            Console.Write("Enter destination (Aircraft Number)");
            string destination = Console.ReadLine();

            airportManager.DispatchToAircraft(vehicle, destination);
        }

        public void CompleteTurnaround()
        {
            Console.WriteLine("=========COMPLETE TURNAROUND=========================");
            Console.Write("Enter Aircraft Number to complete turnaround:");
            string aircraftNumber = Console.ReadLine();

            airportManager.PerformTurnaround(aircraftNumber);
        }

        public void AssignAircraft()
        {
            Console.WriteLine("============================AIRCRAFT ASSIGNMENT====================================");
            Console.Write("Enter Aircraft Number:");
            string aircraftNumber = Console.ReadLine();

            Console.Write("Enter gate to assign:");
            string gate = Console.ReadLine();

            Aircraft plane = airportManager.FindAircraftByNumber(aircraftNumber);
            if(plane == null)
            {
                Console.WriteLine("Aircraft not found.");
                return;
            }
            try
            {
                airportManager.assignGate(gate,plane);
                Console.WriteLine($"Aircraft {aircraftNumber} assigned to {gate}");
            }
            catch(FormatException)
            {
                Console.WriteLine($"Could not assign gate");
            }
        }

        private static void OnFlightDelayed(object sender, FlightDelayedEventArgs e)
        {
            string message =
            $"[{DateTime.Now:HH:mm:ss}] " +
            $"Flight {e.flightNumber} delayed by " +
            $"{e.delayedMinutes} minutes. " +
            $"Reason: {e.reason}";

            eventLog.Add(message);

            Console.WriteLine(message);
        }

        private static void OnTurnaroundCompleted(object sender, TurnaroundCompletedEventArgs e)
        {
            string message =
            $"[{DateTime.Now:HH:mm:ss}] " +
            $"Turnaround completed for flight {e.flightNumber}.";

            eventLog.Add(message);

            Console.WriteLine(message);
        }

        public static void OnLowFuelWarning(object sender, LowFuelEventArgs e)
        {
            string message =
            $"[{DateTime.Now:HH:mm:ss}]" +
            $"LOW FUEL WARNING: Aircraft {e.AircraftNumber} registered with {e.FuelLevel}% fuel";

            eventLog.Add(message);

            Console.WriteLine(message);
        }

        public static void SubscribeToEvents(AirportManager airportManager)
        {
            airportManager.FlightDelayed += OnFlightDelayed;
            airportManager.TurnaroundCompleted += OnTurnaroundCompleted;
            airportManager.LowFuelWarning += OnLowFuelWarning;
        }

        public static void ViewEventLog()
        {
            Console.WriteLine();
            Console.WriteLine("===== Event LOG =====");

            if(eventLog.Count == 0)
            {
                Console.WriteLine("No events have been recorded.");
                return;
            }

            foreach(string message in eventLog)
            {
                Console.WriteLine(message);
            }

            Console.WriteLine("=========================");
        }
    }
}





