using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TMS.Flights.Commands
{
    public class SampleJobCommand : IRequest
    {
        public class Handler : IRequestHandler<SampleJobCommand>
        {
            private readonly ILogger<Handler> logger;

            public Handler(ILogger<Handler> logger)
            {
                this.logger = logger;
            }

            public Task<Unit> Handle(SampleJobCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Executing Sample Job");

                return Unit.Task;
            }
        }
    }
}