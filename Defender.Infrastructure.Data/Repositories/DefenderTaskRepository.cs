using Defender.Domain.Core.Models;
using Defender.Domain.Interfaces;
using Defender.Infrastructure.Data.Contexts;

namespace Defender.Infrastructure.Data.Repositories;

public class DefenderTaskRepository : Repository<DefenderTask>, IDefenderTaskRepository
{
    public DefenderTaskRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}