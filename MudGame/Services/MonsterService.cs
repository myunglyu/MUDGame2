using MudGame.Models;
using MudGame.Data;

namespace MudGame.Services;

public class MonsterService
{
    // Dependency injection
    private readonly ApplicationDbContext _context;
    public MonsterService(ApplicationDbContext context)
    {
        _context = context;
    }
    // Dependency injection

}