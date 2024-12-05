using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MudGame.Controllers;
using MudGame.Data;
using MudGame.Models;
using MudGame.Services;

namespace MudGame.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICharacterService _characterService;
    private readonly IGameService _gameService;
    private readonly GameController _gameController;

    // Inject UserManager into the ChatHub to handle current user information
    public ChatHub(UserManager<IdentityUser> userManager, ICharacterService characterService, IGameService gameService, GameController gameController)
    {
        _userManager = userManager;
        _characterService = characterService;
        _gameController = gameController;
    }

    public Task SendMessage(string name, string message)
    {
        if (Context.User == null)
        {
            return Task.CompletedTask;
        } else {
            var userName = _userManager.GetUserName(Context.User);
            var chatName = $"{name}(ID:{userName})";
            return Clients.All.ReceiveMessage(chatName, message);
        }
    }

    public async Task SendGameCommand(string characterId, string message)
    {
        await _gameController.GameCommand(Context.User, characterId, message);
    }


    // public async Task StoreCharacterInfo(Guid characterId)
    // {
    //     var user = await _userManager.GetUserAsync(Context.User);
    //     var character = await _characterService.SelectCharacterAsync(user, characterId);
    //     if (!string.IsNullOrEmpty(user.Id))
    //     {
    //         ActiveCharacter[user.Id] = character.Name;
    //         await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{user.Id}");
    //         await ((IClientProxy)Clients.Group($"user_{user.Id}")).SendAsync("CharacterUpdated", character);
    //         System.Console.WriteLine($"Character info stored for {user.Id}");
    //     }
    //     System.Console.WriteLine("StoreCharacterInfo called");
    //     System.Console.WriteLine(user.Id, ActiveCharacter[user.Id]);
    // }

    // public async Task GetCharacterInfo()
    // {
    //     var user = await _userManager.GetUserAsync(Context.User);
    //     if (!string.IsNullOrEmpty(user.Id) && ActiveCharacter.TryGetValue(user.Id, out var character))
    //     {
    //         await ((IClientProxy)Clients.Caller).SendAsync("CharacterUpdated", character);
    //     }
    // }
}   