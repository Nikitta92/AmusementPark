using Microsoft.EntityFrameworkCore;
using Model.Domain;

namespace Model.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("AmusementPark");

        modelBuilder
            .Entity<Visitor>()
            .ToTable("Visitor");

        modelBuilder
            .Entity<Visitor>()
            .Property(x => x.RegistrationDate)
            .HasConversion(x => x, x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
        base.OnModelCreating(modelBuilder);
    }
}