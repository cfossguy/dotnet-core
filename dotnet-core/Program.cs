using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

var serviceName = "MyCompany.MyProduct.MyService";
var serviceVersion = "1.0.0";


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetryTracing(b =>
{
    b.AddJaegerExporter(o =>
    {
        o.AgentHost = "http://10.100.158.8";
        o.AgentPort = 9411; // use port number here
    });
});

// Configure important OpenTelemetry settings, the console exporter, and automatic instrumentation
builder.Services.AddOpenTelemetryTracing(b =>
{
    b
        .AddSource(serviceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();
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