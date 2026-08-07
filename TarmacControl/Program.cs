using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarmacControl.UI;

namespace TarmacControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //set the program to still keep running. So the exit option was not selected
            bool isRunning = true;

            //while the program is still running display the menu
            while (isRunning)
            {
                //call the method that will display the menu
                DisplayDefaultMenu();

                //ask the user to select the option
                Console.WriteLine("\nInput the number that best described the action you want to perform.\n");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && Enum.IsDefined(typeof(MenuOption), choice))
                {
                    MenuOption selected = (MenuOption)choice;
                    ManageSelection(selected);
                }
            }
        }
        //set a method that will display the default menu
        static void DisplayDefaultMenu()
        {
            //clear the console of any remaining info
            Console.Clear();

            //Start writing the prompt and info for the menu
            Console.WriteLine("===================== Welcome to TarmacControl =====================");
            

            //Display the menu
            foreach(MenuOption option in Enum.GetValues(typeof(MenuOption)))
            {
                Console.WriteLine($"{(int)option}. {option.GetDescription()}"); //call the get description method
            }
        }

        static void ManageSelection(MenuOption option)
            {
                switch (option)
                {
                    case MenuOption.ViewAllAircraft:
                        //<< insert appropriate code >>
                        break;

                    case MenuOption.ViewAllVehicles:
                        //<< insert appropriate code >>
                    break;

                    case MenuOption.RegisterAircraft:
                        //<< insert appropriate code >>
                        break;

                    case MenuOption.DispatchVehicle:
                        //<< insert appropriate code >>
                        break;

                    case MenuOption.AssignAircraft:
                        //<< insert appropriate code >>
                        break;

                    case MenuOption.ViewEventLog:
                        //<< insert appropriate code >>
                        break;

                    case MenuOption.SaveState:
                        //<< insert appropriate code >>
                        break;

                    case MenuOption.LoadState:
                        //insert display of all aircrafts
                        break;

                    case MenuOption.Exit:
                        //insert display of all aircrafts
                        break;

                    default:
                        Console.WriteLine("The option that you have selected is not valid");
                        break;
            }
            }
    }
}
