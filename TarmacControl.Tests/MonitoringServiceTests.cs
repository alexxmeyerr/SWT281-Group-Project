using System;
using System.Reflection;
using System.Threading;
using TarmacControl.Management;
using Xunit;

namespace TarmacControl.Tests
{
    public class MonitoringServiceTests
    {
        private static bool GetRunningFlag(MonitoringService service)
        {
            FieldInfo field = typeof(MonitoringService)
                .GetField("running", BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)field.GetValue(service);
        }

        [Fact]
        public void Start_SetsRunningFlagToTrue()
        {
            // Arrange
            var airportManager = new AirportManager();
            var service = new MonitoringService(airportManager);

            // Act
            service.Start();
            Thread.Sleep(100); // give the background thread a moment to spin up
            bool isRunning = GetRunningFlag(service);

            // Assert
            Assert.True(isRunning);

            service.Stop(); // cleanup
        }

        [Fact]
        public void Stop_SetsRunningFlagToFalse()
        {
            // Arrange
            var airportManager = new AirportManager();
            var service = new MonitoringService(airportManager);
            service.Start();
            Thread.Sleep(100);

            // Act
            service.Stop();
            bool isRunning = GetRunningFlag(service);

            // Assert
            Assert.False(isRunning);
        }

        [Fact]
        public void StartThenStop_DoesNotThrow()
        {
            // Arrange
            var airportManager = new AirportManager();
            var service = new MonitoringService(airportManager);

            // Act
            var exception = Record.Exception(() =>
            {
                service.Start();
                Thread.Sleep(50);
                service.Stop();
            });

            // Assert
            Assert.Null(exception);
        }
    }
}