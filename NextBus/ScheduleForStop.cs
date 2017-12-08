using System;

namespace NextBus
{

    public class ScheduleForStop
    {
        public Siri Siri { get; set; }
    }

    public class Siri
    {
        public Servicedelivery ServiceDelivery { get; set; }
    }

    public class Servicedelivery
    {
        public DateTime ResponseTimestamp { get; set; }
        public Stopmonitoringdelivery[] StopMonitoringDelivery { get; set; }
        public Situationexchangedelivery[] SituationExchangeDelivery { get; set; }
    }

    public class Stopmonitoringdelivery
    {
        public Monitoredstopvisit[] MonitoredStopVisit { get; set; }
        public DateTime ResponseTimestamp { get; set; }
        public DateTime ValidUntil { get; set; }
    }

    public class Monitoredstopvisit
    {
        public Monitoredvehiclejourney MonitoredVehicleJourney { get; set; }
        public DateTime RecordedAtTime { get; set; }
    }

    public class Monitoredvehiclejourney
    {
        public string LineRef { get; set; }
        public string DirectionRef { get; set; }
        public Framedvehiclejourneyref FramedVehicleJourneyRef { get; set; }
        public string JourneyPatternRef { get; set; }
        public string PublishedLineName { get; set; }
        public string OperatorRef { get; set; }
        public string OriginRef { get; set; }
        public string DestinationRef { get; set; }
        public string DestinationName { get; set; }
        public Situationref[] SituationRef { get; set; }
        public bool Monitored { get; set; }
        public Vehiclelocation VehicleLocation { get; set; }
        public float Bearing { get; set; }
        public string ProgressRate { get; set; }
        public string BlockRef { get; set; }
        public string VehicleRef { get; set; }
        public Monitoredcall MonitoredCall { get; set; }
        public Onwardcalls OnwardCalls { get; set; }
        public DateTime OriginAimedDepartureTime { get; set; }
        public string ProgressStatus { get; set; }
    }

    public class Framedvehiclejourneyref
    {
        public string DataFrameRef { get; set; }
        public string DatedVehicleJourneyRef { get; set; }
    }

    public class Vehiclelocation
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }

    public class Monitoredcall
    {
        public DateTime ExpectedArrivalTime { get; set; }
        public DateTime ExpectedDepartureTime { get; set; }
        public Extensions Extensions { get; set; }
        public string StopPointRef { get; set; }
        public int VisitNumber { get; set; }
        public string StopPointName { get; set; }
    }

    public class Extensions
    {
        public Distances Distances { get; set; }
    }

    public class Distances
    {
        public float DistanceFromCall { get; set; }
        public int StopsFromCall { get; set; }
        public string PresentableDistance { get; set; }
        public float CallDistanceAlongRoute { get; set; }
    }

    public class Onwardcalls
    {
    }

    public class Situationref
    {
        public string SituationSimpleRef { get; set; }
    }

    public class Situationexchangedelivery
    {
        public Situations Situations { get; set; }
    }

    public class Situations
    {
        public Ptsituationelement[] PtSituationElement { get; set; }
    }

    public class Ptsituationelement
    {
        public Publicationwindow PublicationWindow { get; set; }
        public string Severity { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Affects Affects { get; set; }
        public Consequences Consequences { get; set; }
        public DateTime CreationTime { get; set; }
        public string SituationNumber { get; set; }
    }

    public class Publicationwindow
    {
        public DateTime StartTime { get; set; }
    }

    public class Affects
    {
        public Vehiclejourneys VehicleJourneys { get; set; }
    }

    public class Vehiclejourneys
    {
        public Affectedvehiclejourney[] AffectedVehicleJourney { get; set; }
    }

    public class Affectedvehiclejourney
    {
        public string LineRef { get; set; }
        public string DirectionRef { get; set; }
    }

    public class Consequences
    {
        public Consequence[] Consequence { get; set; }
    }

    public class Consequence
    {
        public string Condition { get; set; }
    }

}
