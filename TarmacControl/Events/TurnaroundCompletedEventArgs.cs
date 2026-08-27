using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Events
{
    public class TurnaroundCompletedEventArgs:System.EventArgs
    {
        public string flightNumber { get; }

        public TurnaroundCompletedEventArgs(string flightNumber)
        {
            this.flightNumber = flightNumber;
        }
    }
}
