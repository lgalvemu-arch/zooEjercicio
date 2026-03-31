using Microsoft.EntityFrameworkCore;
using ZooMvc.Data.Repositories;
using ZooMvc.Models.Entities;
using ZooMvc.Models.ViewModels;

namespace ZooMvc.Services;

public class CompatibilityService : ICompatibilityService
{
    private readonly IGenericRepository<Jaula> _jaulaRepository;

    public CompatibilityService(IGenericRepository<Jaula> jaulaRepository)
    {
        _jaulaRepository = jaulaRepository;
    }

    public async Task<List<CageCompatibilityViewModel>> GetCageCompatibilityAsync(CancellationToken cancellationToken = default)
    {
        var jaulas = await _jaulaRepository.QueryNoTracking()
            .Include(j => j.Animales)
            .OrderBy(j => j.Codigo)
            .ToListAsync(cancellationToken);

        return jaulas.Select(j =>
        {
            var animales = j.Animales
                .Select(a => new AnimalInCageViewModel
                {
                    AnimalId = a.Id,
                    Nombre = (a.NombrePopular ?? string.Empty).Trim(),
                    Especie = (a.Especie ?? string.Empty).Trim(),
                    Dieta = InferDiet((a.Especie ?? string.Empty).Trim()).ToString()
                })
                .ToList();

            var dietas = animales.Select(a => ParseDiet(a.Dieta)).Distinct().ToList();
            var compatible = IsCompatible(dietas);

            return new CageCompatibilityViewModel
            {
                JaulaId = j.Id,
                CodigoJaula = j.Codigo.Trim(),
                Compatible = compatible,
                Motivo = compatible ? "Compatible" : "La mezcla de dietas incumple la regla del ejercicio.",
                Animales = animales
            };
        }).ToList();
    }

    public static AnimalDietType InferDiet(string especie)
    {
        var text = especie.Trim().ToLowerInvariant();

        if (new[] { "vaca", "oveja", "caballo", "jirafa", "cebra", "elefante", "ciervo", "conejo", "tortuga" }.Contains(text))
            return AnimalDietType.Herbivoro;

        if (new[] { "leon", "tigre", "lobo", "aguila", "serpiente", "cocodrilo", "hiena", "pantera" }.Contains(text))
            return AnimalDietType.Carnivoro;

        if (new[] { "oso", "cerdo", "chimpance", "mapache", "gallina" }.Contains(text))
            return AnimalDietType.Omnivoro;

        return AnimalDietType.Desconocida;
    }

    private static AnimalDietType ParseDiet(string dieta)
        => Enum.TryParse<AnimalDietType>(dieta, out var result) ? result : AnimalDietType.Desconocida;

    private static bool IsCompatible(List<AnimalDietType> dietas)
    {
        var hasHerbivoro = dietas.Contains(AnimalDietType.Herbivoro);
        var hasCarnivoro = dietas.Contains(AnimalDietType.Carnivoro);
        var hasOmnivoro = dietas.Contains(AnimalDietType.Omnivoro);

        if (hasCarnivoro && hasHerbivoro)
            return false;

        if (hasOmnivoro && hasHerbivoro)
            return false;

        return true;
    }
}
