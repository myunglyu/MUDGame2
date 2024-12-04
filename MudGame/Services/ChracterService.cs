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

    public async Task<bool> CreateCharacterAsync(IdentityUser user, Character newCharacter)
    {
        newCharacter.Id = Guid.NewGuid();
        newCharacter.User = user;

        _context.Characters.Add(newCharacter);

        var saveResult = await _context.SaveChangesAsync();
        return saveResult == 1;
    }

    public async Task<Character[]> GetCharactersAsync(IdentityUser user)
    {
        var characters = await _context.Characters.Where(x => x.User == user).ToArrayAsync();
        return characters;
    }

    public Task DeleteCharacterAsync(IdentityUser user, Character character)
    {
        throw new NotImplementedException();
    }
}