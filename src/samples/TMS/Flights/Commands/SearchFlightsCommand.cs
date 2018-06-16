using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using Zek.Shared;

namespace TMS.Flights.Commands
{
    /// <summary>
    /// Basic flights search
    /// Based on Amadeus API: https://developers.amadeus.com/self-service/category/198/api-doc/4/api-docs-and-example/10002
    /// </summary>
    public class SearchFlightsCommand : IRequest<Result<FlightOffer[]>>
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public DateTimeOffset? ReturnDate { get; set; }

        public class Handler : IRequestHandler<SearchFlightsCommand, Result<FlightOffer[]>>
        {
            public Task<Result<FlightOffer[]>> Handle(SearchFlightsCommand request, CancellationToken cancellationToken)
            {
                // Assume 1-day trip when no return date informed
                var returnDate = request.ReturnDate ?? request.DepartureDate.AddDays(1);

                var flights = new []
                {
                    new FlightOffer
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        TotalPrice = 199.98M,
                        TotalTaxes = 59.99M,
                        Segments = new []
                        {
                            new FlightOfferSegment
                            {
                                Departure = request.Origin,
                                DepartureDate = request.DepartureDate,
                                Arrival = request.Destination,
                                ArrivalDate = request.DepartureDate.AddHours(1)
                            },
                            new FlightOfferSegment
                            {
                                Departure = request.Destination,
                                DepartureDate = returnDate,
                                Arrival = request.Origin,
                                ArrivalDate = returnDate.AddHours(1)
                            }
                        }
                    }
                };

                return Task.FromResult(Result.Ok(flights));
            }
        }

        public class Validator : AbstractValidator<SearchFlightsCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Origin).NotEmpty().Length(3, 3);
                RuleFor(x => x.Destination).NotEmpty().Length(3, 3);

                RuleFor(x => x.DepartureDate).NotEmpty().Custom(DepartureShouldBeInFuture);
                RuleFor(x => x.ReturnDate).Must(ReturnDateMustBeAfterDepartureDate).WithMessage("ReturnDate must be after DepartureDate");
            }

            private static void DepartureShouldBeInFuture(DateTimeOffset date, CustomContext context)
            {
                if (date < Clock.Now)
                {
                    context.AddFailure("DepartureDate should be in future");
                }
            }

            private static bool ReturnDateMustBeAfterDepartureDate(SearchFlightsCommand command, DateTimeOffset? date)
            {
                if (!date.HasValue)
                {
                    return true;
                }

                return date.Value > command.DepartureDate;
            }
        }
    }
}