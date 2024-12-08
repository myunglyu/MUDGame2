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
using System.Security.Claims;

namespace MudGame.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly ILogger<GameController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICharacterService _characterService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IGameService _gameService;
    private readonly IMonsterService _monsterService;

    public GameController(ILogger<GameController> logger,
        ApplicationDbContext context, 
        UserManager<IdentityUser> userManager, 
        ICharacterService characterService,
        IHubContext<ChatHub> hubContext,
        IGameService gameService,
        IMonsterService monsterService)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _characterService = characterService;
        _hubContext = hubContext;
        _gameService = gameService;
        _monsterService = monsterService;
    }

    [ValidateAntiForgeryToken]
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

        var character = await _characterService.GetCharacterAsync(user, characterId);
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
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "<System>", message);
    }

    public async Task SpawnMonster()
    {
        var monster = _monsterService.SpawnMonster();
        if (monster != null)
        {
            var spawnedMonster = await monster;
            await SendGameMessage($"A {spawnedMonster.Name} appears! (Level {spawnedMonster.Level})");
        }
    }

    // [HttpPost]
    // public async Task<IActionResult> StartEncounter()
    // {
    //     await SpawnMonster();
    //     return Ok();
    // }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task GameCommand(ClaimsPrincipal userPrincipal, string characterId, string command)
    {   
        var user = await _userManager.GetUserAsync(userPrincipal);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var character = await _characterService.GetCharacterAsync(user, Guid.Parse(characterId));
        var result = await _gameService.ProcessCommand(character, command);
        await SendGameMessage(result);
    }

}