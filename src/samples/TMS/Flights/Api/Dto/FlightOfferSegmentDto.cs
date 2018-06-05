using System;

namespace TMS.Flights.Api.Dto
{
    public class FlightOfferSegmentDto
    {
        public string Departure { get; set; }
        public DateTimeOffset DepartureDate { get; set; }

        public string Arrival { get; set; }
        public DateTimeOffset ArrivalDate { get; set; }

        public TimeSpan Duration => ArrivalDate - DepartureDate;
    }
}