using System;

namespace TMS.Flights
{
    public class FlightOffer
    {
        public string Id { get; set; }
        public FlightOfferSegment[] Segments { get; set; } = Array.Empty<FlightOfferSegment>();

        public decimal TotalPrice { get; set; }
        public decimal TotalTaxes { get; set; }
    }
}