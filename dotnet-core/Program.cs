using System.Net;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Instrumentation;
using Prometheus;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

var config = new NLog.Config.LoggingConfiguration();
var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logconsole);
NLog.LogManager.Configuration = config;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure important OpenTelemetry settings, the console exporter, and automatic instrumentation
builder.Services.AddOpenTelemetryTracing(b =>
{
    b
    .AddSource("dotnet-core")
    .SetResourceBuilder(
        ResourceBuilder.CreateDefault().AddService("dotnet-core", serviceVersion:"1.1.3pre"))
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddSqlClientInstrumentation()
    .AddJaegerExporter(opt =>
    {
        opt.Endpoint = new Uri("http://10.100.158.8:9411");
        //opt.Protocol = JaegerExportProtocol.HttpBinaryThrift;
        //opt.Protocol = OtlpExportProtocol.Grpc;
        //opt.Protocol = OtlpExportProtocol.HttpProtobuf;
    });
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();
app.UseHttpMetrics();
app.MapControllers();

app.Run();