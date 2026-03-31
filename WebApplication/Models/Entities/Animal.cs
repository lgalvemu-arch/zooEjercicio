namespace ZooMvc.Models.Entities;

public class Animal
{
    public int Id { get; set; }
    public string? Especie { get; set; }
    public string? NombrePopular { get; set; }
    public int? EcosistemaId { get; set; }
    public string? UrlImagen { get; set; }
    public int? JaulaId { get; set; }

    public Jaula? Jaula { get; set; }
    public ICollection<Dosis> Dosis { get; set; } = new List<Dosis>();
}
