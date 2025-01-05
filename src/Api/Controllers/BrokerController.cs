using Api.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrokerController : ControllerBase
{
    private readonly IBrokerService _brokerService;

    public BrokerController(IBrokerService brokerService)
    {
        _brokerService = brokerService;
    }

    [HttpGet("price")]
    public async Task<IActionResult> GetPrice(string symbol)
    {
        var price = await _brokerService.GetCurrentPriceAsync(symbol);
        return Ok(price);
    }

    [HttpPost("order")]
    public async Task<IActionResult> PlaceOrder(string symbol, decimal quantity, string orderType)
    {
        await _brokerService.PlaceOrderAsync(symbol, quantity, orderType);
        return Ok("Order placed successfully");
    }
}