using System;
using System.IO;
using System.Net.Http;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Zek.IntegrationTests
{
    public abstract class BaseIntegrationTest<TStartup> where TStartup : class
    {
        protected BaseIntegrationTest(string contentRoot)
        {
            // More info about MVC Testing https://blogs.msdn.microsoft.com/webdev/2017/12/07/testing-asp-net-core-mvc-web-apps-in-memory/

            var hostBuilder = WebHost.CreateDefaultBuilder(Array.Empty<string>()).
                UseContentRoot(Path.GetFullPath(contentRoot)).
                UseStartup<TStartup>();

            var server = new TestServer(hostBuilder);

            Host = server.Host;

            Client = server.CreateClient();
        }

        protected HttpClient Client { get; }
        protected IWebHost Host { get; }

        protected TService GetService<TService>() => Host.Services.GetRequiredService<TService>();
        protected IMediator Mediator => GetService<IMediator>();
    }
}