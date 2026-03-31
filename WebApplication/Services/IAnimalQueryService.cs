using ZooMvc.Models.ViewModels;

namespace ZooMvc.Services;

public interface IAnimalQueryService
{
    Task<List<AnimalCardViewModel>> GetAnimalCardsAsync(string? especie = null, CancellationToken cancellationToken = default);
}
