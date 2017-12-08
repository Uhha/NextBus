using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace NextBus
{
    public static class Operations
    {
        public static Stop[] StopsNearMe(float lat, float lng, float span = 0.005f)
        {
            //http://bustime.mta.info/api/where/stops-for-location.xml?lat=40.748433&lon=-73.985656&latSpan=0.005&lonSpan=0.005&key=90812fb6-f00c-4b89-8b77-1cda6b2830cf
            //client.BaseAddress = new Uri(Configuration.AccessPoint);
            //client.DefaultRequestHeaders.Add("key", Configuration.ApiKey);

            string parameters = $"where/stops-for-location.json?lat={lat}&lon={lng}&latSpan={span}&lonSpan={span}&key={ConfigurationValues.ApiKey}";
            HttpResponseMessage response = HttpClientWrapper.GetClient().GetAsync(ConfigurationValues.AccessPoint + parameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<StopsForLocation>().Result;
                if (data.code == 200)
                {
                    return data.data.stops;
                }
            }
            return null;
        }

        public static Monitoredstopvisit[] ScheduleForStop(string stopid)
        {
            //http://bustime.mta.info/api/siri/stop-monitoring.json?key=90812fb6-f00c-4b89-8b77-1cda6b2830cf&OperatorRef=MTA&MonitoringRef=302376
            string parameters = $"siri/stop-monitoring.json?key={ConfigurationValues.ApiKey}&OperatorRef=MTA&MonitoringRef={stopid}";
            HttpResponseMessage response = HttpClientWrapper.GetClient().GetAsync(ConfigurationValues.AccessPoint + parameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<ScheduleForStop>().Result;
                return data.Siri.ServiceDelivery.StopMonitoringDelivery?[0].MonitoredStopVisit;
            }
            return null;
        }

        internal static Monitoredstopvisit[] ScheduleForStop(Stop stop)
        {
            return ScheduleForStop(stop.id);
        }

        public static Location GetGeoCoordinatesFromAddress(string address)
        {
            string parameters = $"maps.googleapis.com/maps/api/geocode/json?address={address}&sensor=false";
            HttpResponseMessage response = HttpClientWrapper.GetClient().GetAsync(parameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<GeoCoordinates>().Result;
                if (data.status == "OK")
                {
                    return new Location { lat = data.results[0].geometry.location.lat, lng = data.results[0].geometry.location.lng };
                } 
                else return new Location();
            }
            return new Location();
        }

        public static void GetMapImage(float lat, float lng)
        {
            string parameters = $"https://maps.googleapis.com/maps/api/staticmap?center={lat},{lng}&zoom=18&size=600x600&key={ConfigurationValues.GoogleStaticMapsKey}&style=feature:poi|visibility:off";
            byte[] response = HttpClientWrapper.GetClient().GetByteArrayAsync(parameters).Result;
            throw new NotImplementedException();
        }

        public static void UpdateStations()
        {
            //http://web.mta.info/developers/data/nyct/bus/google_transit_bronx.zip
            //http://web.mta.info/developers/data/nyct/bus/google_transit_brooklyn.zip
            //http://web.mta.info/developers/data/nyct/bus/google_transit_manhattan.zip
            //http://web.mta.info/developers/data/nyct/bus/google_transit_queens.zip
            //http://web.mta.info/developers/data/nyct/bus/google_transit_staten_island.zip
            throw new NotImplementedException();
        }

        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            string a = await content.ReadAsStringAsync();
            T obj = JsonConvert.DeserializeObject<T>(a);
            return obj;
        }
    }

}
