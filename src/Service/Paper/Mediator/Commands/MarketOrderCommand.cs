using System;
using Data.DTOs;
using MediatR;

namespace Service.Paper.Mediator.Commands;

public record MarketOrderCommand(Guid PortfolioId, string Symbol, decimal Quantity) : IRequest<OrderFulfillmentResponse>;