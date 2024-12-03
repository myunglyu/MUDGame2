using Microsoft.AspNetCore.Identity;

namespace MudGame.Models;

public class Character
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string CharacterName { get; set; }
}