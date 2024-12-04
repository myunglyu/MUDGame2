// Controllers/CharacterController.cs
using Microsoft.AspNetCore.Mvc;
using MudGame.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MudGame.Services;

namespace MudGame.Controllers
{
    [Authorize]
    public class CharacterController : Controller
    {
        private readonly ICharacterService _characterService;
        private readonly UserManager<IdentityUser> _userManager;

        public CharacterController(ICharacterService characterService, UserManager<IdentityUser> userManager)
        {
            _characterService = characterService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CharacterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharacterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var character = new Character
            {
                Name = model.Name,
                Class = model.Class,
                Strength = model.Strength,
                Intelligence = model.Intelligence,
                Dexterity = model.Dexterity
            };

            var result = await _characterService.CreateCharacterAsync(user, character);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Error creating character");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // public async Task<IActionResult> Index()
        // {
        //     var user = await _userManager.GetUserAsync(User);
        //     if (user == null)
        //     {
        //         return NotFound();
        //     }

        //     var characters = await _characterService.GetCharactersAsync(user);
        //     return View(characters);
        // }
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