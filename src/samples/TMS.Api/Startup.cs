using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Zek.Shared.Extensions;

namespace TMS
{
    public class Startup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddZek();
            
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseZek();
        }
    }
}