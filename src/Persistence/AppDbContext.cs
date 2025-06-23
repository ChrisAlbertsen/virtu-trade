using System.Threading.Tasks;
using Data.AuthModels;
using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Trade> Trades { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    public DbSet<UserPortfolioAccess> UserPortfolioAccess { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Application");

        modelBuilder.Entity<Portfolio>(portfolio =>
        {
            portfolio
                .Property(h => h.Id)
                .ValueGeneratedNever();

            portfolio
                .HasKey(p => p.Id);
        });


        modelBuilder.Entity<Holding>(holding =>
        {
            holding
                .Property(h => h.Id)
                .ValueGeneratedNever();

            holding
                .HasKey(h => h.Id);

            holding
                .Property(h => h.PortfolioId)
                .IsRequired();
        });


        modelBuilder.Entity<Trade>(trade =>
        {
            trade
                .Property(h => h.Id)
                .ValueGeneratedNever();

            trade
                .HasKey(t => t.Id);

            trade
                .HasOne(t => t.Portfolio)
                .WithMany(p => p.Trades)
                .HasForeignKey(t => t.PortfolioId);
        });

        modelBuilder.Entity<UserPortfolioAccess>(userPortfolioAccess =>
        {
            userPortfolioAccess
                .HasOne<Portfolio>()
                .WithMany()
                .HasForeignKey(upa => upa.PortfolioId);

            userPortfolioAccess
                .HasOne<User>()
                .WithMany(user => user.UserPortfolioAccesses)
                .HasForeignKey(upa => upa.UserId);

            userPortfolioAccess
                .HasKey(upa => new { upa.PortfolioId, upa.UserId });
        });
    }

    public async Task<int> EnsuredSaveChangesAsync(int minExpectedChanges = 1)
    {
        var affectedRows = await SaveChangesAsync();

        if (affectedRows < minExpectedChanges)
            throw new DbUpdateException(
                $"Expected at least {minExpectedChanges} database changes, but only {affectedRows} were applied.");
        return affectedRows;
    }
}