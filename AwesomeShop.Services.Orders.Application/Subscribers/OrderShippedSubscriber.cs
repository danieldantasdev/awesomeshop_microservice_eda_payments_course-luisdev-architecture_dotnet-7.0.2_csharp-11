using System.Text;
using AwesomeShop.Application.Events;
using AwesomeShop.Core.Enums;
using AwesomeShop.Core.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AwesomeShop.Application.Subscribers;

public class OrderShippedSubscriber : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string Queue = "orders-service/order-created";
    private const string RoutingKeySubscribe = "order-shipped";
    private readonly IServiceProvider _serviceProvider;
    private const string Exchange = "orders-service";
    private const string WarehouseExchange = "warehouse-service";

    public OrderShippedSubscriber(IServiceProvider serviceProvider)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = connectionFactory.CreateConnection("orders-service-order-shipped-consumer");

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(Exchange, "direct", false, true, null);
        _channel.ExchangeDeclare(WarehouseExchange, "direct", false, true, null);

        _channel.QueueDeclare(
            Queue,
            false,
            false,
            true,
            null);

        _channel.QueueBind(Queue, WarehouseExchange, RoutingKeySubscribe);

        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);
            var orderShippedEvent = JsonConvert.DeserializeObject<OrderShippedEvent>(contentString);

            Console.WriteLine($"Message OrderShippedEvent received with Id {orderShippedEvent.Id}");

            UpdateOrderStatus(orderShippedEvent);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(Queue, false, consumer);

        return Task.CompletedTask;
    }

    private void UpdateOrderStatus(OrderShippedEvent @event)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetService<IOrderRepository>();

        repository?.UpdateOrderStatus(@event.Id, OrderStatusEnum.Shipped);
    }
}