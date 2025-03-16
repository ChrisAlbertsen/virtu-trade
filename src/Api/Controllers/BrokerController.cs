using System;
using System.Threading.Tasks;
using Data.DTOs.HistoricalPrice;
using Data.DTOs.Orders;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrokerController(
    IBrokerDataService brokerDataService,
    IBrokerOrderService brokerOrderService)
    : ControllerBase
{
    [HttpGet("prices/current")]
    public async Task<IActionResult> GetCurrentPrice(string symbol)
    {
        var price = await brokerDataService.GetCurrentPriceAsync(symbol);
        return Ok(price);
    }

    [HttpGet("prices/historical")]
    public async Task<IActionResult> GetHistoricalPrice([FromQuery] HistoricalPriceParams historicalPriceParams)
    {
        var historicalPriceResponse = await brokerDataService.GetHistoricalPriceAsync(historicalPriceParams);
        return Ok(historicalPriceResponse);
    }

    [HttpPost("orders/execute-market-order")]
    public async Task<IActionResult> ExecuteMarketOrder(MarketOrderParams marketOrderParams)
    {
        var orderFulfillmentResponse = await brokerOrderService.ExecuteMarketOrder(marketOrderParams);
        return Ok(orderFulfillmentResponse);
    }
}