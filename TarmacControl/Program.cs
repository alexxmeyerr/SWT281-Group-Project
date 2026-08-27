using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarmacControl.Entities;
using TarmacControl.Management;
using TarmacControl.UI;

namespace TarmacControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //set the program to still keep running. So the exit option was not selected
            bool isRunning = true;

            //passes the airportmanger to the monitoringSetvice and starts the service on a background thread
            ConsoleMenu menu = new ConsoleMenu();

            ConsoleMenu.SubscribeToEvents(menu.AirportManager);

            //creates monitoringService and give it the AiportManger used by the menu
            MonitoringService monitoring = new MonitoringService(menu.AirportManager); 

            //to start the background thread in the Monitoring service
            monitoring.Start();

            //while the program is still running display the menu
            while (isRunning)
            {
                DisplayDefaultMenu();
                Console.WriteLine("\nInput the number that best described the action you want to perform.\n");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && Enum.IsDefined(typeof(MenuOption), choice))
                {
                    MenuOption selected = (MenuOption)choice;
                    isRunning = menu.ManageSelection(selected);
                }
            }

        }
        //set a method that will display the default menu
        static void DisplayDefaultMenu()
        {
            Console.WriteLine("\n===================== Welcome to TarmacControl =====================");
            Console.WriteLine("1. View all aircrafts");
            Console.WriteLine("2. View all ground vehicles");
            Console.WriteLine("3. Register new flight (creates Aircraft)");
            Console.WriteLine("4. Register new vehicle (creates vehicle)");
            Console.WriteLine("5. Dispatch a ground vehicle to an aircraft");
            Console.WriteLine("6. Assign aircraft to gate/runway");
            Console.WriteLine("7. Complete turnaround for an aircraft");
            Console.WriteLine("8. View system event log");
            Console.WriteLine("9. View Operational Reports");
            Console.WriteLine("10. Exit");
        }
    }
}
