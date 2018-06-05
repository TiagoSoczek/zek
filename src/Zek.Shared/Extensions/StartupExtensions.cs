using System;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Zek.Shared.Api.Filters;

namespace Zek.Shared.Extensions
{
    public static class StartupExtensions
    {
        private static Assembly startupAssembly;

        public static void UseZek(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public static IServiceCollection AddZek<T>(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            startupAssembly = typeof(T).Assembly;

            services.AddMvc(options => options.ConfigureZekFilters())
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddZekValidation();

            services.SuppressModelStateInvalidFilter();

            services.AddAutoMapper();

            Mapper.AssertConfigurationIsValid();

            services.AddMediatR(startupAssembly);

            return services;
        }

        public static IServiceCollection SuppressModelStateInvalidFilter(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            return services;
        }

        public static IMvcBuilder AddZekValidation(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssembly(startupAssembly));

            return builder;
        }

        public static void ConfigureZekFilters(this MvcOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Filters.Add(typeof(ValidatorActionFilter));
            options.Filters.Add(typeof(HandleErrorFilterAttribute));
        }
    }
}