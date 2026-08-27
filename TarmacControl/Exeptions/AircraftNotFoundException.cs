using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Exeptions
{
    internal class AircraftNotFoundException: Exception
    {
        public AircraftNotFoundException(string message) : base(message)
        {
        }
    }
}
