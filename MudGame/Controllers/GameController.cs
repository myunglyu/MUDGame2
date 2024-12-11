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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MudGame.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly ILogger<GameController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly InMemoryDbContext _inMemoryDbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICharacterService _characterService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IGameService _gameService;

    public GameController(ILogger<GameController> logger,
        ApplicationDbContext context, 
        UserManager<IdentityUser> userManager, 
        ICharacterService characterService,
        IHubContext<ChatHub> hubContext,
        IGameService gameService,
        InMemoryDbContext inMemoryDbContext)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _characterService = characterService;
        _hubContext = hubContext;
        _gameService = gameService;
        _inMemoryDbContext = inMemoryDbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Character");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(Guid characterId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
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

    [HttpPost]
    public async Task SpawnMonster(string roomId)
    {
        var monster = await _gameService.SpawnMonster(Guid.Parse(roomId));
        await SendGameMessage($"{monster.Name} has entered the room!");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<string> GameCommand(ClaimsPrincipal userPrincipal, string characterId, string command, string roomId)
    {   
        var user = await _userManager.GetUserAsync(userPrincipal);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var character = await _characterService.GetCharacterAsync(user, Guid.Parse(characterId));
        var room = await _inMemoryDbContext.Rooms.FindAsync(Guid.Parse(roomId));
        return await ProcessCommand(character, command, room);
    }

    public async Task<string> ProcessCommand(Character character, string command, Room room)
    {
        string[] processedCommand = command.ToLower().Split(" ");
        if (processedCommand[0] == "/attack"){
            if (processedCommand.Length == 2){
                return await _gameService.BattleAsync(character, processedCommand[1], room);
            }
            else{
                return "Monster not found!";
            }
        }
        if (processedCommand[0] == "/here"){
            return await _gameService.GetRoomInfoAsync(room);
        }
        if (processedCommand[0] == "/move"){
            if (processedCommand.Length == 1){
                return await _gameService.MoveAsync(character, room);
            }
            else{
                return await _gameService.MoveAsync(character, processedCommand[1], room);
            }
        }
        else {
            return "Invalid command!";
        }
    }
}