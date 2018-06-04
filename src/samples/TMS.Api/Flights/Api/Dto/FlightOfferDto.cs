using System;

namespace TMS.Flights.Api.Dto
{
    public class FlightOfferDto
    {
        public string Id { get; set; }
        public FlightOfferSegmentDto[] Segments { get; set; } = Array.Empty<FlightOfferSegmentDto>();

        public decimal TotalPrice { get; set; }
        public decimal TotalTaxes { get; set; }
    }
}