using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : DbContext(options)
{
    public DbSet<Trade> Trades { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Holding> Holdings { get; set; }
}