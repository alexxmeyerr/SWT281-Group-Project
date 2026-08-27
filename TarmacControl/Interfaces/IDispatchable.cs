using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Interfaces
{
    internal interface IDispatchable
    {
        bool isAvailable { get; }
        void Dispatch(string destination);

    }
}
