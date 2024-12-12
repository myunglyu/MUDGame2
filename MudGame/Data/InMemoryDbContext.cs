using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MudGame.Models;

namespace MudGame.Data;

public class InMemoryDbContext : DbContext
{
    private readonly ApplicationDbContext _sourceDb;

    public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options, ApplicationDbContext sourceDb)
        : base(options)
    {
        _sourceDb = sourceDb;
    }
    
    public DbSet<Room> Rooms { get; set; }

    // Method to sync data from SQLite
    public async Task SyncFromDatabase()
    {
        // Clear existing data
        Rooms.RemoveRange(Rooms);

        // Load from SQLite
        var rooms = await _sourceDb.Rooms.ToListAsync();

        // Add to in-memory
        await Rooms.AddRangeAsync(rooms);
        
        await SaveChangesAsync();
    }

    // Method to sync data to SQLite
    public async Task SyncToDatabase()
    {
        var rooms = await Rooms.ToListAsync();
        foreach (var room in rooms) _sourceDb.Update(room);
        
        await _sourceDb.SaveChangesAsync();
    }
}
