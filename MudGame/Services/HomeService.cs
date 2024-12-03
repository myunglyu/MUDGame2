using MudGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MudGame.Services;

public class HomeService
{
    public async Task<IActionResult> GetUserAsync(UserManager<IdentityUser> userManager, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(user);
    }
}