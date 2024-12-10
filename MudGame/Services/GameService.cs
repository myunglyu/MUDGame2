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

    public async Task<Monster> SpawnMonster()
    {
        var monsters = await _context.Monsters.ToArrayAsync();
        var monster = monsters[new Random().Next(0, monsters.Length)];
        return monster;
    }
}