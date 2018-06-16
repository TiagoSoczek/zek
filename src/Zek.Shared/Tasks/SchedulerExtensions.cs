using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Zek.Shared.Tasks
{
    public static class SchedulerExtensions
    {
        public static IServiceCollection AddScheduler(this IServiceCollection services)
        {
            return services
                    .AddSingleton<IHostedService, SchedulerHostedService>()
                    .AddTransient<MediatorTask>();
        }

        public static IServiceCollection AddMediatorTask<T>(this IServiceCollection services, string schedule, Func<IRequest> buildRequest = null) where T : IRequest, new()
        {
            if (buildRequest == null)
            {
                buildRequest = () => new T();
            }

            return services.AddMediatorTask(schedule, typeof(T).Name, buildRequest);
        }

        public static IServiceCollection AddMediatorTask(this IServiceCollection services, string schedule, string description, Func<IRequest> buildRequest)
        {
            return services.AddSingleton<IScheduledTask>(provider => provider.GetRequiredService<MediatorTask>().Configure(schedule, description, buildRequest));
        }
    }
}