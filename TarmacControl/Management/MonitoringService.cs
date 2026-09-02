using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TarmacControl.Management
{
    internal class MonitoringService
    {
        private readonly AirportManager airportManager; //stores reference to the AiportManager to access aiport data need for monitoring
        
        //storing the thread that will run monitoring checks
        private Thread monitorThread;
        private volatile bool running;  //to control whether monitoring loop should continue or stop

        public MonitoringService(AirportManager airportManager)
        {
            this.airportManager = airportManager;
        }

        //starting background threading
        public void Start()
        {
            running = true; //allow moinitoring loop to run
            monitorThread = new Thread(RunMonitoring); //create thread to execute the monitoring method
            monitorThread.IsBackground = true; //set monitoriung thread as a background thread
            monitorThread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        //the background thread
        private void RunMonitoring()
        {
            while (running) //continoously does monitoring checks while the service is running
            {
                try
                {
                    //testing thread
                    /* 
                    *Console.WriteLine($"MONITOR THREAD] Running on thread {Thread.CurrentThread.ManagedThreadId}"); */
                    CheckLowFuel();
                    CheckIdleVehicles();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Monitoring] Unexpected error: {ex.Message}");
                }
                finally
                {
                    Thread.Sleep(15000);    //waits 15 seconds before checking again
                }
            }
        }

        //get aircrafts with low fuel levels and dispolay an alert for each one 
        private void CheckLowFuel()
        {
            var lowFuelAircraft = airportManager.GetLowFuelAircraft();
            foreach (var aircraft in lowFuelAircraft)
            {
                Console.WriteLine($"\n[ALERT {DateTime.Now:HH:mm:ss}]: {aircraft.AircraftNumber} is low on Fuel ({aircraft.FuelLevel}%)");
            }
        }

        //gets all idle vehicles and displsys the amount currently vailable
        private void CheckIdleVehicles()
        {
            var idleVehicles = airportManager.GetIdleVehicles();
            if(idleVehicles.Count > 0)
            {
                Console.WriteLine($"[INFO ({DateTime.Now:HH:mm:ss})]: {idleVehicles.Count} vehicle(s) currently idle and available.");
            }
        }
    }
}
