using System;

namespace Data.DTOs.Orders;

public class OrderFulfillmentResponse
{
    public required Guid Id { get; set; }
    public required string Symbol { get; set; }
    public required decimal Price { get; set; }
    public required decimal Quantity { get; set; }
}