namespace AwesomeShop.Core.MessageBus.Interfaces;

public interface IMessageBusService
{
    void Publish(object data, string routingKey);
}