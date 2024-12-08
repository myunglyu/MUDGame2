using System;
using System.Collections.Generic;
using MudGame.Models;
using SQLitePCL;
using MudGame.Data;
using Microsoft.EntityFrameworkCore;
using MudGame.Controllers;
using Microsoft.AspNetCore.SignalR;
using MudGame.Hubs;
using NuGet.Protocol.Core.Types;

namespace MudGame.Services;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;
    public GameService(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    private async Task SendGameMessage(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "<System>", message);
    }

    public async Task<bool> BattleAsync(Character character, Monster monster)
    {
        await SendGameMessage($"{character.Name} is battling {monster.Name}");
        var characterAttack = character.Strength + character.Level - monster.Defense;
        if (characterAttack < 1)
        {
            characterAttack = 1;
        }
        var monsterAttack = monster.Attack + monster.Level - character.Level;
        if (monsterAttack < 1)
        {
            monsterAttack = 1;
        }
        var characterHPLeft = character.HitPoints;
        var monsterHPleft = monster.HitPoints;
        while (characterHPLeft > 0 && monsterHPleft > 0)
        {
            characterHPLeft-= monsterAttack;
            monsterHPleft -= characterAttack;
            await SendGameMessage($"{character.Name} attacks {monster.Name} for {characterAttack} damage [{monsterHPleft}/{monster.HitPoints}]");
            await SendGameMessage($"{monster.Name} attacks {character.Name} for {monsterAttack} damage [{characterHPLeft}/{character.HitPoints}]");
        }
        return characterHPLeft > monsterHPleft;
    }
        public Task<Monster[]> GetActiveMonsters()
    {
        throw new NotImplementedException();
    }

    public async Task<string> ProcessCommand(Character character, string command)
    {
        var _character = character;
        var _command = command.Split(" ");
        if (_command[0] == "/attack"){
            System.Console.WriteLine($"{_character.Name} is attacking {_command[1]}");
            var monster = await _context.Monsters.FirstOrDefaultAsync(x => x.Name.ToLower() == _command[1].ToLower());
            if (monster != null){
                var result = await BattleAsync(_character, monster);
                if (result){
                    return $"{_character.Name} defeated the {monster.Name}!";
                }
                else{
                    return $"{_character.Name} was defeated by the {monster.Name}!";
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