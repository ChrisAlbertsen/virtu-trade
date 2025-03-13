using System.Threading.Tasks;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : DbContext(options)
{
    public DbSet<Trade> Trades { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Portfolio>()
            .Property(h => h.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Holding>()
            .Property(h => h.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Trade>()
            .Property(h => h.Id)
            .ValueGeneratedNever();
    }
    
    public async Task<int> EnsuredSaveChangesAsync(int expectedChanges = 1)
    {
        var affectedRows = await SaveChangesAsync();

        if (affectedRows < expectedChanges)
        {
            throw new DbUpdateException($"Expected at least {expectedChanges} database changes, but only {affectedRows} were applied.");
        }
        return affectedRows;
    }
}