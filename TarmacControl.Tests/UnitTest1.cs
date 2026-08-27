using Xunit;
using TarmacControl.Management;
using TarmacControl.Events;

namespace TarmacControl.Tests
{
    public class AirportManagerEventTests
    {
        [Fact]
        public void TriggerFlightDelayed_RaisesFlightDelayedEvent()
        {
            AirportManager manager = new AirportManager();
            bool fired = false;
            string flightNum = string.Empty;

            manager.FlightDelayed += (sender, e) =>
            {
                fired = true;
                flightNum = e.flightNumber;
            };

            manager.TriggerFlightDelayed("SA281", "Weather", 15);

            Assert.True(fired);
            Assert.Equal("SA281", flightNum);
        }

        [Fact]
        public void TriggerTurnaroundCompleted_RaisesTurnaroundCompletedEvent()
        {
            AirportManager manager = new AirportManager();
            bool fired = false;
            string flightNum = string.Empty;

            manager.TurnaroundCompleted += (sender, e) =>
            {
                fired = true;
                flightNum = e.flightNumber;
            };

            manager.TriggerTurnaroundCompleted("SA281");

            Assert.True(fired);
            Assert.Equal("SA281", flightNum);
        }

        [Fact]
        public void TriggerLowFuelWarning_RaisesLowFuelWarningEvent()
        {
            AirportManager manager = new AirportManager();
            bool fired = false;
            string aircraftNum = string.Empty;
            int fuel = 0;

            manager.LowFuelWarning += (sender, e) =>
            {
                fired = true;
                aircraftNum = e.AircraftNumber;
                fuel = e.FuelLevel;
            };

            manager.TriggerLowFuelWarning("AC-100", 10);

            Assert.True(fired);
            Assert.Equal("AC-100", aircraftNum);
            Assert.Equal(10, fuel);
        }
    }
}