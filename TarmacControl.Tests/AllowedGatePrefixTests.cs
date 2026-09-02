using TarmacControl.Entities;
using Xunit;
namespace TarmacControl.Tests;

public class AllowedGatePrefixTests
{
    [Fact]
    public void AllowedGatePrefix_PassengerJet_ReturnsCorrectPrefix()
    {
        // Arrange
        Aircraft aircraft = new PassengerJet(AircraftNumber: "ACW1", flightNumber: "NZ1244",
            status: "Arriving", assignedGate: "G1", fuelLevel: 80, boarding: true, baggageWeight: 200);

        // Act
        char result = aircraft.AllowedGatePrefix;

        // Assert
        Assert.Equal('A', result);
    }

    [Fact]
    public void AllowedGatePrefix_CargoPlane_ReturnsCorrectPrefix()
    {
        // Arrange
        Aircraft aircraft = new CargoPlane(AircraftNumber: "ACW2", flightNumber: "NZ1245",
            status: "Arriving", assignedGate: "G2", fuelLevel: 80, loadWeight: 300);

        // Act
        char result = aircraft.AllowedGatePrefix;

        // Assert
        Assert.Equal('C', result);
    }

    [Fact]
    public void AllowedGatePrefix_PrivateJet_ReturnsCorrectPrefix()
    {
        // Arrange 
        Aircraft aircraft = new PrivateJet(AircraftNumber: "ACW3", flightNumber: "NZ1246",
            status: "Arriving", assignedGate: "G3", fuelLevel: 80, flightDuration: 2, passengercount: 4);

        // Act
        char result = aircraft.AllowedGatePrefix;

        // Assert
        Assert.Equal('D', result);
    }
}

