using Microsoft.AspNetCore.Identity;

namespace MudGame.Models;

public class Character
{
    public Guid Id { get; set; }
    public IdentityUser User { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Dexterity { get; set; }
}