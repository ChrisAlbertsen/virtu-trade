using Data.Models;
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

    [HttpGet("prices/current")]
    public async Task<IActionResult> GetCurrentPrice(string symbol)
    {
        var price = await _brokerService.GetCurrentPriceAsync(symbol);
        return Ok(price);
    }

    [HttpGet("prices/historical")]
    public async Task<IActionResult> GetHistoricalPrice([FromQuery] HistoricalPriceParams historicalPriceParams)
    {
        var historicalPriceResponse = await _brokerService.GetHistoricalPriceAsync(historicalPriceParams);
        return Ok(historicalPriceResponse);
    }

    [HttpPost("order")]
    public async Task<IActionResult> PlaceOrder(string symbol, decimal quantity, string orderType)
    {
        await _brokerService.PlaceOrderAsync(symbol, quantity, orderType);
        return Ok("Order placed successfully");
    }
}