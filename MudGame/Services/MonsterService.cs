using MudGame.Models;
using MudGame.Data;

namespace MudGame.Services;

public class MonsterService : IMonsterService
{
    // Dependency injection
    private readonly ApplicationDbContext _context;
    public MonsterService(ApplicationDbContext context)
    {
        _context = context;
    }
    // Dependency injection

    public async Task<bool> CreateMonsterAsync(Monster monster)
    {
        monster.Id = Guid.NewGuid();

        _context.Monsters.Add(monster);
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }
    
    public async Task<bool> EditMonsterAsync(Monster monster)
    {
        _context.Monsters.Update(monster);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteMonsterAsync(Monster monster)
    {
        _context.Monsters.Remove(monster);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}