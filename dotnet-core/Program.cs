using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace dotnet_core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceName = "MyCompany.MyProduct.MyService";
            var serviceVersion = "1.0.0";
            var builder = WebApplication.CreateBuilder(args);
            
            // Configure important OpenTelemetry settings, the console exporter, and automatic instrumentation
            builder.Services.AddOpenTelemetryTracing(b =>
            {
                b
                    .AddConsoleExporter()
                    .AddSource(serviceName)
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();
            });
            
            var app = builder.Build();
            var httpClient = new HttpClient();
            app.Run();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}