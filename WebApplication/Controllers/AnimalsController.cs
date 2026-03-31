using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZooMvc.Data;
using ZooMvc.Data.Repositories;
using ZooMvc.Models.Entities;
using ZooMvc.Services;

namespace ZooMvc.Controllers;

public class AnimalsController : Controller
{
    private readonly IGenericRepository<Animal> _repository;
    private readonly ZooDbContext _context;
    private readonly IFeedingService _feedingService;

    public AnimalsController(IGenericRepository<Animal> repository, ZooDbContext context, IFeedingService feedingService)
    {
        _repository = repository;
        _context = context;
        _feedingService = feedingService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await _repository.QueryNoTracking()
            .Include(a => a.Jaula)
            .OrderBy(a => a.Especie)
            .ThenBy(a => a.NombrePopular)
            .ToListAsync(cancellationToken);
        return View(items);
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var animal = await _repository.QueryNoTracking()
            .Include(a => a.Jaula)
                .ThenInclude(j => j!.Zona)
                    .ThenInclude(z => z!.Ecosistema)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (animal is null)
            return NotFound();

        ViewBag.Calorias = await _feedingService.GetCaloriesByAnimalAsync(id, cancellationToken);
        return View(animal);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewBag.Jaulas = await _context.Jaulas.AsNoTracking().OrderBy(j => j.Codigo).ToListAsync(cancellationToken);
        return View(new Animal());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Animal animal, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Jaulas = await _context.Jaulas.AsNoTracking().OrderBy(j => j.Codigo).ToListAsync(cancellationToken);
            return View(animal);
        }

        await _repository.AddAsync(animal, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var animal = await _repository.GetByIdAsync(id, cancellationToken);
        if (animal is null)
            return NotFound();

        ViewBag.Jaulas = await _context.Jaulas.AsNoTracking().OrderBy(j => j.Codigo).ToListAsync(cancellationToken);
        return View(animal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Animal animal, CancellationToken cancellationToken)
    {
        if (id != animal.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            ViewBag.Jaulas = await _context.Jaulas.AsNoTracking().OrderBy(j => j.Codigo).ToListAsync(cancellationToken);
            return View(animal);
        }

        _repository.Update(animal);
        await _repository.SaveChangesAsync(cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var animal = await _repository.QueryNoTracking().Include(a => a.Jaula).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (animal is null)
            return NotFound();

        return View(animal);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var animal = await _repository.GetByIdAsync(id, cancellationToken);
        if (animal is not null)
        {
            _repository.Remove(animal);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        return RedirectToAction(nameof(Index));
    }
}
