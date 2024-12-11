namespace MudGame.Models;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Exits { get; set; }
    public List<Guid>? Monsters { get; set; }
    public List<Guid>? Characters { get; set; }
}