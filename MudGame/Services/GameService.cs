using System;
using System.Collections.Generic;
using MudGame.Models;
using SQLitePCL;
using MudGame.Data;
using Microsoft.EntityFrameworkCore;

namespace MudGame.Services;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _context;
    public GameService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<bool> BattleAsync(Character character, Monster monster)
    {
        System.Console.WriteLine($"{character.Name} is battling {monster.Name}");
        return Task.FromResult(true);
    }
        public Task<Monster[]> GetActiveMonsters()
    {
        throw new NotImplementedException();
    }
    public Task<Monster> SpawnMonster()
    {
        var monster = new Monster
        {
            Id = Guid.NewGuid(),
            Name = "Goblin",
            HitPoints = 10,
            Level = 1
        };
        return Task.FromResult(monster);
    }

    public async Task<string> ProcessCommand(Character character, string command)
    {
        var _character = character;
        var _command = command.Split(" ");
        if (_command[0] == "/attack"){
            System.Console.WriteLine($"{_character.Name} is attacking {_command[1]}");
            var monster = await _context.Monsters.FirstOrDefaultAsync(x => x.Name.ToLower() == _command[1]);
            if (monster != null){
                var result = await BattleAsync(_character, monster);
                if (result){
                    return $"{_character.Name} defeated the {monster.Name}!";
                }
                else{
                    return $"{_character.Name} were defeated by the {monster.Name}!";
                }
            }
            else{
                return "Monster not found!";
            }
        } else {
            return "Invalid command!";
        }
    }
}