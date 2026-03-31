using Microsoft.EntityFrameworkCore;
using ZooMvc.Data.Repositories;
using ZooMvc.Models.Entities;
using ZooMvc.Models.ViewModels;

namespace ZooMvc.Services;

public class FeedingService : IFeedingService
{
    private readonly IGenericRepository<Alimento> _alimentoRepository;
    private readonly IGenericRepository<Dosis> _dosisRepository;

    public FeedingService(IGenericRepository<Alimento> alimentoRepository, IGenericRepository<Dosis> dosisRepository)
    {
        _alimentoRepository = alimentoRepository;
        _dosisRepository = dosisRepository;
    }

    public async Task<List<FoodOrderSuggestionViewModel>> GetFoodOrderSuggestionsAsync(CancellationToken cancellationToken = default)
    {
        var alimentos = await _alimentoRepository.QueryNoTracking()
            .Include(a => a.Proveedor)
            .OrderBy(a => a.Nombre)
            .ToListAsync(cancellationToken);

        var dosisAgrupadas = await _dosisRepository.QueryNoTracking()
            .GroupBy(d => d.AlimentoId)
            .Select(g => new { AlimentoId = g.Key, Total = g.Sum(x => x.Cantidad ?? 0) })
            .ToListAsync(cancellationToken);

        var dosisPorAlimento = dosisAgrupadas.ToDictionary(x => x.AlimentoId, x => x.Total);

        return alimentos
            .Where(a => (a.Stock ?? 0) < (a.StockMinimo ?? 0))
            .Select(a =>
            {
                var totalDosis = dosisPorAlimento.TryGetValue(a.Id, out var cantidad) ? cantidad : 0;
                var pedido = totalDosis > 5 ? totalDosis : 5;

                return new FoodOrderSuggestionViewModel
                {
                    AlimentoId = a.Id,
                    Alimento = a.Nombre ?? string.Empty,
                    StockActual = a.Stock ?? 0,
                    StockMinimo = a.StockMinimo ?? 0,
                    DosisNecesarias = totalDosis,
                    CantidadAPedir = pedido,
                    Proveedor = a.Proveedor?.Nombre ?? string.Empty
                };
            })
            .ToList();
    }

    public async Task<int> GetCaloriesByAnimalAsync(int animalId, CancellationToken cancellationToken = default)
    {
        var total = await _dosisRepository.QueryNoTracking()
            .Include(d => d.Alimento)
            .Where(d => d.AnimalId == animalId)
            .SumAsync(d => (d.Cantidad ?? 0) * (d.Alimento != null ? (d.Alimento.Calorias ?? 0) : 0), cancellationToken);

        return total;
    }
}
