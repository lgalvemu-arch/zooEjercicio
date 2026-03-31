using ZooMvc.Models.ViewModels;

namespace ZooMvc.Services;

public interface IFeedingService
{
    Task<List<FoodOrderSuggestionViewModel>> GetFoodOrderSuggestionsAsync(CancellationToken cancellationToken = default);
    Task<int> GetCaloriesByAnimalAsync(int animalId, CancellationToken cancellationToken = default);
}
