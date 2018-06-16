using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TMS.Flights.Commands;
using Zek.Shared.Extensions;
using Zek.Shared.Tasks;
using Zek.Shared.Tasks.Cron;

namespace TMS
{
    public class Startup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddZek<Startup>();

            services.AddMediatorTask<SampleJobCommand>(CronExpressions.EveryMinute);

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseZek();
        }
    }
}