using Microsoft.AspNetCore.Mvc;
using ZooMvc.Models.ViewModels;
using ZooMvc.Services;

namespace ZooMvc.Controllers;

public class ManagementController : Controller
{
    private readonly IFeedingService _feedingService;
    private readonly ICompatibilityService _compatibilityService;
    private readonly IManagementService _managementService;

    public ManagementController(IFeedingService feedingService, ICompatibilityService compatibilityService, IManagementService managementService)
    {
        _feedingService = feedingService;
        _compatibilityService = compatibilityService;
        _managementService = managementService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var vm = new ManagementDashboardViewModel
        {
            InversionTotalComida = await _managementService.GetFoodInvestmentAsync(cancellationToken),
            CostePorAnimal = await _managementService.GetAnimalCostsAsync(cancellationToken),
            PedidosSugeridos = await _feedingService.GetFoodOrderSuggestionsAsync(cancellationToken),
            Compatibilidades = await _compatibilityService.GetCageCompatibilityAsync(cancellationToken)
        };

        return View(vm);
    }
}
