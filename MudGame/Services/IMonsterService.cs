using MudGame.Models;

namespace MudGame.Services;

public interface IMonsterService
{
    public Task<bool> CreateMonsterAsync(Monster monster);
    public Task<Monster> SpawnMonster();
}