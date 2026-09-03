using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarmacControl.Entities
{
    internal class PassengerJet: Aircraft
    {
        private enum BoardingStatus { NotBoarding, FirstBoarding, Boarding, LastBoarding, UnknownStatus }
        private bool boarding;
        private double baggageWeight;

        public PassengerJet(string AircraftNumber,string flightNumber, string status, string assignedGate, int fuelLevel, bool boarding, double baggageWeight): base(AircraftNumber,flightNumber, status, assignedGate, fuelLevel)
        {
            this.boarding = boarding;
            this.baggageWeight = baggageWeight;
        }
        public override char AllowedGatePrefix => 'A';
        public override void Turnaround()
        {
            Console.WriteLine("Baggage");
            Console.WriteLine("Fueling");
            Console.WriteLine("Boarding");
        }

        public override string ToString()
        {
            return base.ToString() + $"Baggage weight: {baggageWeight}kg\n";
        }

        public BoardingStatus BoardingPassengers(List<int> boardingNumbers)
        {
            int processedBoardingNumber = 0;
        
            if (boardingNumbers != null)
            {
                foreach(int boardingNumber in boardingNumbers)
                {
                    processedBoardingNumber++;     
                }
        
                if (processedBoardingNumber == 0)
                    return BoardingStatus.NotBoarding;
                else if (processedBoardingNumber == 1)
                    return BoardingStatus.FirstBoarding;
                else if (processedBoardingNumber > 1)
                    return BoardingStatus.Boarding;
                else if (processedBoardingNumber == boardingNumbers.Count)
                    return BoardingStatus.LastBoarding;
                else
                    return BoardingStatus.NotBoarding;
            }
            else if(boardingNumbers == null)
                return BoardingStatus.UnknownStatus;
            
            return BoardingStatus.UnknownStatus;
        }
    }
}
