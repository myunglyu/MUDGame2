using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MudGame.Controllers;
using MudGame.Data;

namespace MudGame.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly UserManager<IdentityUser> _userManager;

    // Inject UserManager into the ChatHub to handle current user information
    public ChatHub(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public Task SendMessage(string message)
    {
        if (Context.User == null)
        {
            return Task.CompletedTask;
        } else {
            var userName = _userManager.GetUserName(Context.User);
            return Clients.All.ReceiveMessage(userName, message);
        }
    }
}   