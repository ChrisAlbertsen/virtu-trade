using Service.Paper.Mediator.Operations;

namespace Service.Interfaces;

public interface ITradeExecutionService
{
    public void ExecuteTradeAsync(BaseOrderCommand command);
}