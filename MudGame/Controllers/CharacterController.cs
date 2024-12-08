using Microsoft.AspNetCore.Mvc;
using MudGame.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MudGame.Services;
using SQLitePCL;
using MudGame.Data;

namespace MudGame.Controllers
{
    [Authorize]
    public class CharacterController : Controller
    {
        private readonly ICharacterService _characterService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CharacterController(ICharacterService characterService, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _characterService = characterService;
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new CharacterViewModel{
                Characters = await _characterService.GetCharactersAsync(user)
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }
            int count = _context.Characters.Count(x => x.UserId.ToString().Equals(userId));
            System.Console.WriteLine("Character Count: ", count);
            if (count >= 6)
            {
                // Add message to TempData
                TempData["ErrorMessage"] = "You have reached the maximum limit of 6 characters.";
                return RedirectToAction("Index", "Home");
            }
            else { return View(new Character()); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Character model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _characterService.CreateCharacterAsync(user, model);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Error creating character");
                return View(model);
            }

            return RedirectToAction(nameof(GameController.Index), "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var character = await _characterService.GetCharacterAsync(user, id);
            if (character == null)
            {
                return NotFound();
            }

            var result = await _characterService.DeleteCharacterAsync(user, character);
            if (!result)
            {
                TempData["ErrorMessage"] = "Unable to delete the character. Please try again later.";
                return View(character);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

// using Microsoft.AspNetCore.Mvc;
// using MudGame.Models;
// using Microsoft.AspNetCore.Identity;
// using MudGame.Data;
// using MudGame.Services;
// using Microsoft.AspNetCore.Authorization;

// namespace MudGame.Controllers;

// public class CharacterController : Controller
// {
//     private readonly ICharacterService _characterService;
//     private readonly UserManager<IdentityUser> _userManager;

//     public CharacterController(ICharacterService characterService, UserManager<IdentityUser> userManager)
//     {
//         _characterService = characterService;
//         _userManager = userManager;
//     }

//     public IActionResult Index()
//     {

//         return View();
//     }

//     [HttpGet]
//     public IActionResult Create()
//     {
//         return View(new CharacterViewModel());
//     }

//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create(CharacterViewModel model)
//     {
//         if (!ModelState.IsValid)
//         {
//             return View(model);
//         }

//         var user = await _userManager.GetUserAsync(User);
//         if (user == null)
//         {
//             return NotFound();
//         }

//         // TODO: Add character creation logic using _characterService
        
//         return RedirectToAction(nameof(Index));
//     }
//     // public async Task<IActionResult> Create(Character newCharacter)
//     // {
//     //     if (ModelState.IsValid)
//     //     {
//     //         var user = await _userManager.GetUserAsync(User);
//     //         var successful = await _characterService.CreateCharacterAsync(user, newCharacter);
//     //         if (!successful)
//     //         {
//     //             return BadRequest("Could not add item.");
//     //         }
//     //     }
//     //     return RedirectToAction("Index");
//     // }

//     // public async Task<IActionResult> Select(IdentityUser user)
//     // {
//     //     var characters = await _characterService.GetCharactersAsync(user);
//     //     return View(characters);
//     // }
// }