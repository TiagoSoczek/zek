using System;

namespace TMS.Flights
{
    public class FlightOfferSegment
    {
        public string Departure { get; set; }
        public DateTimeOffset DepartureDate { get; set; }

        public string Arrival { get; set; }
        public DateTimeOffset ArrivalDate { get; set; }

        public TimeSpan Duration => ArrivalDate - DepartureDate;
    }
}