using System;
using System.Threading.Tasks;
using Data.DTOs.Interfaces;

namespace Service.Interfaces;

public interface IPaperTradeCatchService
{
    Task CatchTrade(IOrder order);
}