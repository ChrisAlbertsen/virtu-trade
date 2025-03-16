using System;
using System.Collections.Generic;

namespace Data.Entities;

public class Portfolio
{
    public required Guid Id { get; set; }
    public required decimal Cash { get; set; }
    public required decimal ReservedCash { get; set; }

    public required ICollection<Holding> Holdings { get; set; } = new List<Holding>();
}