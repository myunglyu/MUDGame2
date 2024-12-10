using MudGame.Models;

namespace MudGame.Services;

public interface IMonsterService
{
    public Task<bool> CreateMonsterAsync(Monster monster);
    public Task<bool> EditMonsterAsync(Monster monster);
    public Task<bool> DeleteMonsterAsync(Monster monster);
}