using Microsoft.AspNetCore.Mvc;
using ZooMvc.Services;

namespace ZooMvc.ViewComponents;

public class AnimalViewComponent : ViewComponent
{
    private readonly IAnimalQueryService _animalQueryService;

    public AnimalViewComponent(IAnimalQueryService animalQueryService)
    {
        _animalQueryService = animalQueryService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? especie)
    {
        var items = await _animalQueryService.GetAnimalCardsAsync(especie);
        return View(items);
    }
}
