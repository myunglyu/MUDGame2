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
using Humanizer;

namespace MudGame.Services;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _context;
    private readonly InMemoryDbContext _inMemoryDbContext;
    private readonly IHubContext<ChatHub> _hubContext;
    public GameService(ApplicationDbContext context, IHubContext<ChatHub> hubContext, InMemoryDbContext inMemoryDbContext)
    {
        _context = context;
        _hubContext = hubContext;
        _inMemoryDbContext = inMemoryDbContext;
    }

    private async Task SendGameMessage(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "<System>", message);
    }

    public async Task<string> BattleAsync(Character character, string monsterName, Room room)
    {
        var monster = await _context.Monsters.FirstOrDefaultAsync(x => x.Name.ToLower() == monsterName.ToLower());
        
        if (monster == null || !room.Monsters.Contains(monster.Id))
        {
            return "Monster not found";
        }
        
        await SendGameMessage($"{character.Name} attacked {monster.Name}");
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
        if (characterHPLeft > monsterHPleft) return $"{character.Name} defeated {monster.Name}!";
        else return $"{character.Name} were defeated by {monster.Name}!";
    }

    public async Task<Monster> SpawnMonster(Guid roomId)
    {
        var monsters = await _context.Monsters.ToArrayAsync();
        var monster = monsters[new Random().Next(0, monsters.Length)];

        var room = await _inMemoryDbContext.Rooms.FindAsync(roomId);
        if (room == null)
        {
            System.Console.WriteLine($"Room with ID {roomId} not found");
        }
        room.Monsters ??= new List<Guid>();
        room.Monsters.Add(monster.Id);
        
        await _inMemoryDbContext.SaveChangesAsync();
        return monster;
    }
}