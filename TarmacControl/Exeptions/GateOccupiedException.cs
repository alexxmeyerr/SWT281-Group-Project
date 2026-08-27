using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Exeptions
{
    internal class GateOccupiedException: Exception
    {
        public GateOccupiedException(string message) : base(message)
        { 
        
        }
    }
}
