using System;
using System.Collections.Generic;
using Data.AuthModels;
using Newtonsoft.Json;

namespace Data.Entities;

public class Portfolio
{
    public required Guid Id { get; set; }
    public required decimal Cash { get; set; }
    public required decimal ReservedCash { get; set; }
    public ICollection<Holding> Holdings { get; init; } = new List<Holding>();
    public ICollection<Trade> Trades { get; init; } = new List<Trade>();
}