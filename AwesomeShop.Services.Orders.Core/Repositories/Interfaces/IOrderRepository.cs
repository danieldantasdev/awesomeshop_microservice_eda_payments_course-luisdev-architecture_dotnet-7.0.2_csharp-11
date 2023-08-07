using AwesomeShop.Core.Enums;

namespace AwesomeShop.Core.Repositories.Interfaces;

public interface IOrderRepository
{
    void UpdateOrderStatus(int id, OrderStatusEnum status);
}