using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TarmacControl.Entities;
using TarmacControl.Exeptions;
using TarmacControl.Management;
using Xunit;
namespace TarmacControl.Tests
{
    public class AirportManager_ConcurrencyTests
        {

            private static List<Aircraft> GetAircraftList(AirportManager manager)
            {
                FieldInfo field = typeof(AirportManager)
                    .GetField("aircraftList", BindingFlags.NonPublic | BindingFlags.Instance);
                return (List<Aircraft>)field.GetValue(manager);
            }

            [Fact]
            public async Task TestConcurrency_OfAssigningPlanesToGates()
            {
                // Arrange
                AirportManager manager = new AirportManager();
                const int generatedInstances = 10;
                const string gate = "A1";

                List<PassengerJet> testPlanes = Enumerable.Range(0, generatedInstances)
                    .Select(i => new PassengerJet($"AC{i}", $"FL{i}", "Arrived", "A1", 80, false, 500))
                    .ToList();

                var successes = new ConcurrentBag<string>();
                var failures = new ConcurrentBag<Exception>();

                // Act
                var tasks = testPlanes.Select(plane => Task.Run(() =>
                {
                    try
                    {
                        manager.assignGate(gate, plane);
                        successes.Add(plane.AircraftNumber);
                    }
                    catch (GateOccupiedException ex)
                    {
                        failures.Add(ex);
                    }
                }));

                await Task.WhenAll(tasks);

                // Assert
                Assert.True(successes.Count == 1,
                    $"Expected to return only one successful assignment to a gate, but got {successes.Count}. " +
                    "This indicates the check-then-act race in assignGate() allowed double-booking.");
                Assert.Equal(generatedInstances - 1, failures.Count);
            }

            [Fact]
            public async Task ConcurrencyCheck_AddingTwoAircraftsWithDuplicateData()
            {
                // Arrange
                AirportManager manager = new AirportManager();
                const int attempts = 20;
                const string sharedNumber = "DUP01";

                IEnumerable<PassengerJet> planes = Enumerable.Range(0, attempts)
                    .Select(i => new PassengerJet(sharedNumber, $"FL{i}", "Arrived", "A1", 80, false, 500));

                // Act
                IEnumerable<Task> tasks = planes.Select(plane => Task.Run(() => manager.addAircraft(plane)));
                await Task.WhenAll(tasks);

                // Assert
                int stored = GetAircraftList(manager)
                    .Count(a => a.AircraftNumber == sharedNumber);

                Assert.True(stored <= 1,
                    $"Only one aircaft with the number '{sharedNumber}' was expected to be stored, but {stored} were found. " +
                    "This indicates the check-then-add race in addAircraft() let duplicates through.");
            }

            [Fact]
            public async Task RetrieveLowFuelAircraft_WhileListIsBeingModified()
            {
            // Arrange
                AirportManager manager = new AirportManager();
                for (int i = 0; i < 50; i++)
                {
                    manager.addAircraft(new PassengerJet($"SEED{i}", $"FL{i}", "Arrived", "A1", 10, false, 500));
                }

                using var cts = new CancellationTokenSource();
                ConcurrentBag<Exception> readExceptions = new ConcurrentBag<Exception>();

                // Act
                Task writer = Task.Run(() =>
                {
                    int i = 0;
                    while (!cts.IsCancellationRequested)
                    {
                        manager.addAircraft(new PassengerJet($"NEW{i}", $"FL{i}", "Arrived", "A1", 10, false, 500));
                        i++;
                    }
                });

                var reader = Task.Run(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        try
                        {
                            manager.GetLowFuelAircraft();
                        }
                        catch (Exception ex)
                        {
                            readExceptions.Add(ex);
                        }
                    }
                });

                await Task.Delay(200);
                cts.Cancel();
                await Task.WhenAll(writer, reader);

                // Assert
                Assert.Empty(readExceptions);
            }    
    }
}


