using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Events
{
    public class FlightDelayedEventArgs:System.EventArgs
    {
        public string flightNumber { get; }
        public string reason { get; }
        public int delayedMinutes { get; }

        public FlightDelayedEventArgs(string flightNumber, string reason, int delayedMinutes)
        {
            this.flightNumber = flightNumber;
            this.reason = reason;
            this.delayedMinutes = delayedMinutes;
        }
    }
}
