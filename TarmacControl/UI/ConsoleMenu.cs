using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//add enums that will hold the menu values for each different menu
enum MenuOption
{
    [Description("View all aircrafts")] //set descriptions for each value in the enum so that we can display sentences
    ViewAllAircraft = 1, //set the first value to have an integer value of 1
    [Description("View all ground vehicles")]
    ViewAllVehicles,
    [Description("Register new flight (creates Aircraft)")]
    RegisterAircraft,
    [Description("Dispatch a ground vehicle to an aircraft")]
    DispatchVehicle,
    [Description("Assign aircraft to gate/runway")]
    AssignAircraft,
    [Description("View system event log")]
    ViewEventLog,
    [Description("Save system state")]
    SaveState,
    [Description("Load system state")]
    LoadState,
    [Description("Exit")]
    Exit
}

namespace TarmacControl.UI
{
    static class ConsoleMenu
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }

}
