using System;
using System.Threading.Tasks;
using Data.Models;
using Service.Interfaces;

namespace Service.PaperBroker;

public class PaperBrokerOrderService : IBrokerOrderService
{
    public Task<OrderFulfillmentResponse> MarketOrder(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
        // create unit test for MarketOrder first

        // get symbol current price

        // check if enough cash on portfolio 
        // reserve cash
        // create trade in database
        // deduct cash

        // consider how trades should impact holdings and portfolio
    }
}