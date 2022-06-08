using Defender.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Defender.Infrastructure.Data.Contexts;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<DefenderTask> DefenderTasks { get; set; }
}