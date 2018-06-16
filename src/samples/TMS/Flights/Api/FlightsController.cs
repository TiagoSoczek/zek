using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Flights.Api.Dto;
using TMS.Flights.Commands;
using Zek.Shared.Api;

namespace TMS.Flights.Api
{
    [Route(RouteConstants.Controller)]
    public class FlightsController : BaseController
    {
        public FlightsController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<FlightOfferDto[]>> Search(SearchFlightsCommand cmd)
        {
            var result = await Mediator.Send(cmd).ConfigureAwait(false);

            return As<FlightOfferDto[]>(result);
        }
    }
}