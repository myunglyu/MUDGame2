using Microsoft.AspNetCore.Mvc;
using MudGame.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MudGame.Services;
using SQLitePCL;
using MudGame.Data;
using Microsoft.AspNetCore.SignalR;
using MudGame.Hubs;

namespace MudGame.Controllers;

[Authorize]
public class MonsterController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMonsterService _monsterService;

    public MonsterController(ApplicationDbContext context, IMonsterService monsterService, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _monsterService = monsterService;
    }

    public IActionResult Index()
    {
        var monsters = _context.Monsters.ToArray();
        var model = new MonsterViewModel
        {
            Monsters = monsters
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Monster());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Monster monster)
    {
    //     var result = await _monsterService.CreateMonsterAsync(monster);

    //     if (!result)
    //     {
    //         ModelState.AddModelError(string.Empty, "Error creating character");
    //         return View(monster);
    //     }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var monster = await _context.Monsters.FindAsync(id);
        if (monster == null)
        {
            return NotFound();
        }

        return View(monster);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Monster monster)
    {
        // var result = await _monsterService.EditMonsterAsync(monster);

        // if (!result)
        // {
        //     ModelState.AddModelError(string.Empty, "Error updating monster");
        //     return View(monster);
        // }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        // var monster = await _context.Monsters.FindAsync(id);
        // if (monster == null)
        // {
        //     return NotFound();
        // }

        // var result = await _monsterService.DeleteMonsterAsync(monster);
        // if (!result)
        // {
        //     TempData["ErrorMessage"] = "Unable to delete the monster. Please try again later.";
        //     return View(monster);
        // }
        return RedirectToAction("Index");
    }
}
