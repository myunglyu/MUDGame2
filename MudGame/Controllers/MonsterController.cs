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
        var result = await _monsterService.CreateMonsterAsync(monster);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Error creating character");
            return View(monster);
        }

        return RedirectToAction("Index");
    }
}