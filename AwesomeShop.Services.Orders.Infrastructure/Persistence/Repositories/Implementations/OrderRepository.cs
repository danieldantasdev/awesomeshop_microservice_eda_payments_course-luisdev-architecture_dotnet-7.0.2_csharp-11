using AwesomeShop.Core.Enums;
using AwesomeShop.Core.Repositories.Interfaces;

namespace AwesomeShop.Infrastructure.Persistence.Repositories.Implementations;

public class OrderRepository : IOrderRepository
{
    public void UpdateOrderStatus(int id, OrderStatusEnum status)
    {
        // Order Status Updated
        throw new NotImplementedException();
    }
}