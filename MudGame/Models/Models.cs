using Microsoft.AspNetCore.Identity;

namespace MudGame.Models;

public class MudGameUser : IdentityUser{
}

public class Characters{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string CharacterName { get; set; }
}