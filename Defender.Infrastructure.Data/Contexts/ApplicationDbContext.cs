using Defender.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Defender.Infrastructure.Data.Contexts;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DefenderTask>()
            .Property(x => x.Id);
    }

    public DbSet<DefenderTask> DefenderTasks { get; set; }
}