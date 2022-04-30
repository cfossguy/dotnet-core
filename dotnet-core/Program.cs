using System.Net;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

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
        ResourceBuilder.CreateDefault().AddService("dotnet-core"))
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddOtlpExporter(opt =>
    {
        opt.Endpoint = new Uri("http://10.100.158.8:4317");
        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
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