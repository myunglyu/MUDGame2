using MudGame.Models;

namespace MudGame.Services;

public interface IGameService{

    public Task<bool> BattleAsync(Character character, Monster monster);
}