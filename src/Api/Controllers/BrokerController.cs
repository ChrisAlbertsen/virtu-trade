using System.Threading.Tasks;
using Data.Entities;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service.Binance;
using Service.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrokerController(IBrokerDataService brokerDataService, IBrokerOrderService brokerOrderService, ApplicationDatabaseContext dbContext)
    : ControllerBase
{
    private readonly ApplicationDatabaseContext _dbContext = dbContext;

    [HttpGet("prices/current")]
    public async Task<IActionResult> GetCurrentPrice(string symbol)
    {
        // dbContext.Books
        //     .Add(new Book() { Content = "some context", Id = 1, Title = "some title" });
        //
        // await dbContext.SaveChangesAsync();
        
        var price = await brokerDataService.GetCurrentPriceAsync(symbol);
        return Ok(price);
    }

    [HttpGet("prices/historical")]
    public async Task<IActionResult> GetHistoricalPrice([FromQuery] HistoricalPriceParams historicalPriceParams)
    {
        var historicalPriceResponse = await brokerDataService.GetHistoricalPriceAsync(historicalPriceParams);
        return Ok(historicalPriceResponse);
    }

    public async Task<IActionResult> CreateMarketOrder(string symbol, decimal quantity)
    {
        var 
    }
}