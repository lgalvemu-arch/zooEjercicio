namespace ZooMvc.Models.ViewModels;

public class ManagementDashboardViewModel
{
    public decimal InversionTotalComida { get; set; }
    public List<AnimalCostViewModel> CostePorAnimal { get; set; } = new();
    public List<FoodOrderSuggestionViewModel> PedidosSugeridos { get; set; } = new();
    public List<CageCompatibilityViewModel> Compatibilidades { get; set; } = new();
}
