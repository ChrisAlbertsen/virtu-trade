using System.Threading.Tasks;
using Data.DTOs.BaseModels;

namespace Service.Interfaces;

public interface ITradeCatchService
{
    Task CatchTrade(BaseOrder order);
}