using ExpertStore.Ordering.Domain;
using ExpertStore.Ordering.Repositories;
using ExpertStore.Ordering.Subscribers;
using ExpertStore.Ordering.Tracing;
using ExpertStore.Ordering.UseCases;
using ExpertStore.SeedWork.Interfaces;
using ExpertStore.SeedWork.RabbitProducer;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTracing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

builder.Services.AddTransient<IUseCase<CreateOrderInput, CreateOrderOutput>, CreateOrder>();
builder.Services.AddTransient<IUseCase<List<ListOrdersOutputItem>>, ListOrders>();
builder.Services.AddTransient<IUpdateOrderPaymentResult, UpdateOrderPaymentResult>();
builder.Services.AddRabbitMessageBus();
builder.Services.AddPaymentSubscriber();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetryTracing(traceProvider =>
{
    traceProvider
        .AddSource(OpenTelemetryExtensions.ServiceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(OpenTelemetryExtensions.ServiceName,
                    serviceVersion: OpenTelemetryExtensions.ServiceVersion))
        .AddAspNetCoreInstrumentation()
        .AddJaegerExporter(exporter =>
        {
            exporter.AgentHost = builder.Configuration["OpenTelemetry:AgentHost"];
            exporter.AgentPort = Convert.ToInt32(builder.Configuration["OpenTelemetry:AgentPort"]);
        });
});

builder.Services.AddSingleton<ITracer>(sp =>
{
    var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var reporter = new RemoteReporter.Builder().WithLoggerFactory(loggerFactory).WithSender(new UdpSender())
        .Build();
    var tracer = new Tracer.Builder(serviceName)
        // The constant sampler reports every span.
        .WithSampler(new ConstSampler(true))
        // LoggingReporter prints every reported span to the logging framework.
        .WithReporter(reporter)
        .Build();
    return tracer;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();