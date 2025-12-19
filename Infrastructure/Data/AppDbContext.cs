using gerenciadorLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace gerenciadorLivraria.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // Entender esse base(options)
    {
    }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasIndex(b => new { b.Title, b.Author })
            .IsUnique();

    }

}
