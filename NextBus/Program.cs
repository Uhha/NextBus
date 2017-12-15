using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NextBus
{
    class Program
    {

        static void Main(string[] args)
        {
            var stop1 = Operations.GetStopById("302375");

            var stops = Operations.StopsNearMe(40.682607f,-73.961773f, 0.005f);
            //var stops = Operations.StopsNearMe(40.767496, -73.691098, 0.015);
            foreach (var stop in stops)
            {
                Console.WriteLine($"{stop.id}: {stop.name}, {stop.direction} -- {string.Join(",", stop.routes.Select(o => o.shortName).ToArray())}");
            }


            //https://maps.googleapis.com/maps/api/staticmap?center=40.682607,-73.961773&zoom=18&size=600x600&key=AIzaSyB8rdVH0Qrcs--Kxj8Bw6pgKAecmR37uUQ&style=feature:poi|visibility:off
            //https://maps.googleapis.com/maps/api/staticmap?center=40.682607,-73.961773&zoom=18&size=600x600&key=AIzaSyB8rdVH0Qrcs--Kxj8Bw6pgKAecmR37uUQ&style=feature:poi|visibility:off&style=feature:transit.station.bus|hue:red

            while (true)
            {
                string stopid = Console.ReadLine();


                var schedules = Operations.ScheduleForStop(stopid);

                foreach (var sched in schedules)
                {
                    TimeSpan ts = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime).Subtract(DateTime.Now);
                    var arrivalTime = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime.Year == 1) ?
                        sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.PresentableDistance :
                        "arriving in " + ts.Minutes + ":" + ts.Seconds + "minutes";
                    Console.WriteLine(
                    sched.MonitoredVehicleJourney.PublishedLineName + ": " +
                        sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.StopsFromCall + " Stops away, " + arrivalTime

                    );
                }

            }
            

        }
    }
}
