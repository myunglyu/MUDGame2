using Microsoft.AspNetCore.Identity;
using MudGame.Models;
using MudGame.Data;

namespace MudGame.Services;

public interface ICharacterService
{
    Task<bool> CreateCharacterAsync(IdentityUser user, Character newCharacter);
    Task<Character[]> GetCharactersAsync(IdentityUser user);
    Task<Character> SelectCharacterAsync(IdentityUser user, Guid characterId);
    // Task DeleteCharacterAsync(IdentityUser user, Character character);
}