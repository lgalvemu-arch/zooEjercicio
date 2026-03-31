using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZooMvc.Data;
using ZooMvc.Models.ViewModels;
using ZooMvc.Services;

namespace ZooMvc.Controllers;

public class HomeController : Controller
{
    private readonly IAnimalQueryService _animalQueryService;
    private readonly ZooDbContext _context;

    public HomeController(IAnimalQueryService animalQueryService, ZooDbContext context)
    {
        _animalQueryService = animalQueryService;
        _context = context;
    }

    public async Task<IActionResult> Index(string? especie, CancellationToken cancellationToken)
    {
        var animales = await _animalQueryService.GetAnimalCardsAsync(especie, cancellationToken);
        var especies = await _context.Animales
            .AsNoTracking()
            .Where(a => a.Especie != null && a.Especie != "")
            .Select(a => a.Especie!.Trim())
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

        return View(new HomeIndexViewModel
        {
            EspecieFiltro = especie,
            Animales = animales,
            EspeciesDisponibles = especies
        });
    }
}
