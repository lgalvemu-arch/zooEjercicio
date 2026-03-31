using Microsoft.EntityFrameworkCore;
using ZooMvc.Models.Entities;

namespace ZooMvc.Data;

public class ZooDbContext : DbContext
{
    public ZooDbContext()
    {
    }

    public ZooDbContext(DbContextOptions<ZooDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=Zoo;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }
    }

    public DbSet<Alimento> Alimentos => Set<Alimento>();
    public DbSet<Animal> Animales => Set<Animal>();
    public DbSet<CategoriaLaboral> CategoriasLaborales => Set<CategoriaLaboral>();
    public DbSet<Dosis> Dosis => Set<Dosis>();
    public DbSet<Ecosistema> Ecosistemas => Set<Ecosistema>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Jaula> Jaulas => Set<Jaula>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Zona> Zonas => Set<Zona>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Alimento>(entity =>
        {
            entity.ToTable("Alimento");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("money");
            entity.Property(e => e.Url).HasColumnName("url").HasMaxLength(255).IsUnicode(false);
            entity.HasOne(e => e.Proveedor)
                .WithMany(p => p.Alimentos)
                .HasForeignKey(e => e.ProveedorId)
                .HasConstraintName("FK_Alimento_Proveedores");
        });

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.ToTable("Animal");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Especie).HasMaxLength(10).IsFixedLength();
            entity.Property(e => e.NombrePopular).HasColumnName("Nombre Popular").HasMaxLength(10).IsFixedLength();
            entity.Property(e => e.UrlImagen).HasColumnName("url imagen").HasMaxLength(255);
            entity.HasOne(e => e.Jaula)
                .WithMany(j => j.Animales)
                .HasForeignKey(e => e.JaulaId)
                .HasConstraintName("FK_Animal_Jaula");
        });

        modelBuilder.Entity<CategoriaLaboral>(entity =>
        {
            entity.ToTable("CategoriasLaborales");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descripcion).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Sueldo).HasColumnType("money");
        });

        modelBuilder.Entity<Dosis>(entity =>
        {
            entity.ToTable("Dosis");
            entity.HasKey(e => e.DosisId);
            entity.Property(e => e.DosisId).HasColumnName("DosisID");
            entity.Property(e => e.AnimalId).HasColumnName("AnimalID");
            entity.Property(e => e.AlimentoId).HasColumnName("AlimentoID");

            entity.HasOne(e => e.Alimento)
                .WithMany(a => a.Dosis)
                .HasForeignKey(e => e.AlimentoId)
                .HasConstraintName("FK_Dosis_Alimento");

            entity.HasOne(e => e.Animal)
                .WithMany(a => a.Dosis)
                .HasForeignKey(e => e.AnimalId)
                .HasConstraintName("FK_Dosis_Animal");
        });

        modelBuilder.Entity<Ecosistema>(entity =>
        {
            entity.ToTable("Ecosistema");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descripcion).HasMaxLength(500).IsUnicode(false);
            entity.Property(e => e.Url).HasColumnName("url").HasMaxLength(255).IsUnicode(false);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("Empleados");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Apodo).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Telefono).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.Cp).HasColumnName("CP").HasMaxLength(5).IsUnicode(false);
            entity.Property(e => e.CategoriasId).HasColumnName("CategoriasId");
            entity.Property(e => e.JaulaId).HasColumnName("JaulaID");

            entity.HasOne(e => e.Categoria)
                .WithMany(c => c.Empleados)
                .HasForeignKey(e => e.CategoriasId)
                .HasConstraintName("FK_Empleados_CategoriasLaborales");

            entity.HasOne(e => e.Jaula)
                .WithMany(j => j.Empleados)
                .HasForeignKey(e => e.JaulaId)
                .HasConstraintName("FK_Empleados_Jaula");
        });

        modelBuilder.Entity<Jaula>(entity =>
        {
            entity.ToTable("Jaula");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Codigo).HasMaxLength(5).IsFixedLength();
            entity.Property(e => e.ZonaId).HasColumnName("ZonaId");
            entity.HasOne(e => e.Zona)
                .WithMany(z => z.Jaulas)
                .HasForeignKey(e => e.ZonaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Jaula_Zona");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ValueGeneratedNever());
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Direccion).HasColumnType("varchar(max)").IsUnicode(false);
        });

        modelBuilder.Entity<Zona>(entity =>
        {
            entity.ToTable("Zona");
            entity.HasKey(e => e.ZonaId);
            entity.Property(e => e.Descripcion).HasMaxLength(50);
            entity.Property(e => e.EcosistemaId).HasColumnName("EcosistemaId");
            entity.HasOne(e => e.Ecosistema)
                .WithMany(ec => ec.Zonas)
                .HasForeignKey(e => e.EcosistemaId)
                .HasConstraintName("FK_Zona_Ecosistema");
        });
    }
}
