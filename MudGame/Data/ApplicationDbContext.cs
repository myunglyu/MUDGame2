﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MudGame.Models;

namespace MudGame.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Monster> Monsters { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Room>().HasData(
            new Room
            {
                Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                Name = "Town Square",
                Description = "Welcome to the Town Square. You see a fountain in the center of the square.",
                Exits = "North,South,East,West",
                Monsters = [Guid.Parse("1888A3D0-F507-4901-AD1D-0E163CDF60A9"), Guid.Parse("1888A3D0-F507-4901-AD1D-0E163CDF60A9")],
                Characters = []
            }
        );
    }
}

