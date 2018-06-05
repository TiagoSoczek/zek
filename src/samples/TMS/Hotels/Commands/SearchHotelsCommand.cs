using System;
using MediatR;
using Zek.Shared;

namespace TMS.Hotels.Commands
{
    // https://developers.amadeus.com/self-service/category/202/api-doc/11/api-docs-and-example/10009
    public class SearchHotelsCommand : IRequest<Result>
    {
        public string CityCode { get; set; }
        public DateTimeOffset CheckInDate { get; set; }
        public DateTimeOffset CheckoutDate { get; set; }
    }
}