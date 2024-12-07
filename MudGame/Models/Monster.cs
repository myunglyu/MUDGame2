using Microsoft.Identity.Client.Extensibility;
using MudGame.Data;

namespace MudGame.Models;

public class Monster {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int HitPoints { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public string? DropItems { get; set; }
}