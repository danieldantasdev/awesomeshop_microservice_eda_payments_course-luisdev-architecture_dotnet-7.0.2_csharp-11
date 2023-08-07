namespace AwesomeShop.Core.Enums;

public enum OrderStatusEnum
{
    StartedAndPaymentPending = 1,
    PreparingForDelivery = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}