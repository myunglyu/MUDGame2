using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MudGame.Models;
using MudGame.Data;
using SQLitePCL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MudGame.Services;
using Microsoft.AspNetCore.SignalR;
using MudGame.Hubs;

namespace MudGame.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly ILogger<GameController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICharacterService _characterService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IGameStateService _gameStateService;

    public GameController(ILogger<GameController> logger,
        ApplicationDbContext context, 
        UserManager<IdentityUser> userManager, 
        ICharacterService characterService,
        IHubContext<ChatHub> hubContext,
        IGameStateService gameStateService)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _characterService = characterService;
        _hubContext = hubContext;
        _gameStateService = gameStateService;
    }

    public async Task<IActionResult> Index(Guid characterId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (characterId == null)
        {
            return RedirectToAction("Index", "Character");
        }

        var character = await _characterService.SelectCharacterAsync(user, characterId);
        if (character == null)
        {
            // Log for debugging
            _logger?.LogWarning($"No character found for user {user.Id}");
            return RedirectToAction("Index", "Character");
        }

        return View(character);
    }

    public async Task SendGameMessage(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Game", message);
    }

    public async Task SpawnMonster()
    {
        var monster = await _gameStateService.SpawnMonsterAsync();
        if (monster != null)
        {
            await SendGameMessage($"A {monster.Name} appears! (Level {monster.Level})");
        }
    }

    [HttpPost]
    public async Task<IActionResult> StartEncounter()
    {
        await SpawnMonster();
        return Ok();
    }
}