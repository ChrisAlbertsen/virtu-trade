using System.Threading.Tasks;
using Data.DTOs.BaseModels;

namespace Service.Interfaces;

public interface IPaperTradeCatchService
{
    Task CatchTrade(BaseOrder order);
}