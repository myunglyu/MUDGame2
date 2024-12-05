using MudGame.Models;
using MudGame.Data;
using Microsoft.EntityFrameworkCore;

namespace MudGame.Services;

public interface IGameStateService
{
    Task<Monster> SpawnMonsterAsync();
    Task<Monster[]> GetActiveMonsters();
}

public class GameStateService : IGameStateService
{
    private readonly ApplicationDbContext _context;
    
    public GameStateService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Monster[]> GetActiveMonsters()
    {
        throw new NotImplementedException();
    }

    public async Task<Monster> SpawnMonsterAsync()
    {
        var monster = new Monster
        {
            Id = Guid.NewGuid(),
            Name = "Goblin",
            HitPoints = 10,
            Level = 1
        };
        return monster!;
    }
}