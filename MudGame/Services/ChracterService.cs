using Microsoft.AspNetCore.Identity;
using MudGame.Models;
using MudGame.Data;
using Microsoft.EntityFrameworkCore;

namespace MudGame.Services;

public class CharacterService : ICharacterService
{
    // Dependency injection
    private readonly ApplicationDbContext _context;
    public CharacterService(ApplicationDbContext context)
    {
        _context = context;
    }
    // Dependency injection

    public async Task<bool> CreateCharacterAsync(IdentityUser user, Character character)
    {
        character.Id = Guid.NewGuid();
        character.User = user;
        character.UserId = user.Id;
        character.Level = 1;
        character.Experience = 0;
        character.HitPoints = 10;
        character.MagicPoints = 10;

        _context.Characters.Add(character);
        var saveResult = await _context.SaveChangesAsync();
        return saveResult > 0;
    }
    
    public async Task<Character[]> GetCharactersAsync(IdentityUser user)
    {
        var characters = await _context.Characters.Where(x => x.UserId == user.Id).ToArrayAsync();
        return characters;
    }

    public async Task<Character> GetCharacterAsync(IdentityUser user, Guid characterId)
    {
        var character = await _context.Characters
            .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == characterId) 
            ?? throw new InvalidOperationException("Character not found");
        return character;
    }

    // public async Task<bool> CreateCharacterAsync(IdentityUser user, Character newCharacter)
    // {
    //     newCharacter.Id = Guid.NewGuid();
    //     newCharacter.User = user;

    //     _context.Characters.Add(newCharacter);

    //     var saveResult = await _context.SaveChangesAsync();
    //     return saveResult == 1;
    // }


    // public Task DeleteCharacterAsync(IdentityUser user, Character character)
    // {
    //     throw new NotImplementedException();
    // }
}