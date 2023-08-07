using AwesomeShop.Application.Events;
using AwesomeShop.Application.Models;
using AwesomeShop.Core.MessageBus.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeShop.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMessageBusService _busService;

    public OrdersController(IMessageBusService busService)
    {
        _busService = busService;
    }

    [HttpPost]
    public IActionResult Post(AddOrderInputModel model)
    {
        var id = new Random().Next(1, 10000); // Poderia ser chamada de um Repositório, por exemplo.

        var @event = new OrderCreatedEvent(id, model);

        _busService.Publish(@event, "order-created");

        return Accepted();
    }
}