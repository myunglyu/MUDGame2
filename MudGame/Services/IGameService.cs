using MudGame.Models;

namespace MudGame.Services;

public interface IGameService{

    public Task<bool> BattleAsync(Character character, Monster monster);
    public Task<Monster[]> GetActiveMonsters();
    public Task<string> ProcessCommand(Character character, string command);
}