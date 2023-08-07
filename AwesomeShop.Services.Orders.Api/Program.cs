using AwesomeShop.Application.Subscribers;
using AwesomeShop.Core.MessageBus.Interfaces;
using AwesomeShop.Core.Repositories.Interfaces;
using AwesomeShop.Infrastructure.Persistence.Repositories.Implementations;
using AwesomeShop.Infrastructure.Services.Implementations.MessageBus.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMessageBusService, RabbitMqService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddHostedService<OrderShippedSubscriber>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();