using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Zek.Shared.Tasks
{
    public class MediatorTask : IScheduledTask
    {
        private readonly IMediator mediator;
        private Func<IRequest> buildRequest;

        public MediatorTask(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public MediatorTask Configure(string schedule, string description, Func<IRequest> buildRequest)
        {
            Schedule = schedule;
            Description = description;

            this.buildRequest = buildRequest;

            return this;
        }

        public string Description { get; private set; }
        public string Schedule { get; private set; }

        public Task ExecuteAsync(CancellationToken cancellationToken) => mediator.Send(buildRequest(), cancellationToken);
    }
}