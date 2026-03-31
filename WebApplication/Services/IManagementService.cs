using ZooMvc.Models.ViewModels;

namespace ZooMvc.Services;

public interface IManagementService
{
    Task<List<AnimalCostViewModel>> GetAnimalCostsAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetFoodInvestmentAsync(CancellationToken cancellationToken = default);
}
