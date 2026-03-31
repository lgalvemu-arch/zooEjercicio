using ZooMvc.Models.ViewModels;

namespace ZooMvc.Services;

public interface ICompatibilityService
{
    Task<List<CageCompatibilityViewModel>> GetCageCompatibilityAsync(CancellationToken cancellationToken = default);
}
