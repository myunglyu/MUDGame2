using System;
using System.Collections.Generic;
using MudGame.Models;

namespace MudGame.Services;

public class GameService : IGameService
{
    public Task<bool> BattleAsync(Character character, Monster monster)
    {
        System.Console.WriteLine($"{character.Name} is battling {monster.Name}");
        return Task.FromResult(true);
    }
}