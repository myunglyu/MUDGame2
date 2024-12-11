using MudGame.Models;

namespace MudGame.Services;

public interface IGameService{

    public Task<string> BattleAsync(Character character, string monsterName, Room room);
    // public Task<string> ProcessCommand(Character character, string command);
    public Task<Monster> SpawnMonster(Guid roomId);
    // public Task<Monster[]> GetActiveMonsters();
    public Task<string> GetRoomInfoAsync(Room room);
    public Task<string> MoveAsync(Character character, Room room);
    public Task<string> MoveAsync(Character character, string direction, Room room);
}