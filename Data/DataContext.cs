using Microsoft.EntityFrameworkCore;
using AuthService.Models;

namespace AuthService.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RevokedToken> RevokedTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

        modelBuilder.Entity<RevokedToken>()
            .HasIndex(rt => rt.Token)
            .IsUnique();

        // Aseguramos que la propiedad Id sea de tipo GUID y sea la clave primaria
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .HasDefaultValueSql("NEWID()")  // Asigna un GUID por defecto si no se proporciona uno
            .ValueGeneratedOnAdd();          // Genera el valor automáticamente al agregar el usuario

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);  // Establece que la propiedad Id es la clave primaria
    }
}
