using ExpertStore.Ordering.Domain;
using ExpertStore.Ordering.Repositories;
using ExpertStore.Ordering.UseCases;
using ExpertStore.SeedWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

builder.Services.AddTransient<IUseCase<CreateOrderInput, CreateOrderOutput>, CreateOrder>();
builder.Services.AddTransient<IUseCase<List<ListOrdersOutputItem>>, ListOrders>();
builder.Services.AddRabbitMessageBus();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
