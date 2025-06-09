using webApi.Models.Base;
using Microsoft.EntityFrameworkCore;
using webApi.Models;
using System.Reflection.Emit;
using CarrocinhaDoBem.Api.Context.Mappings;
using webApi.Context.Mappings;

namespace webApi.Context;


public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<Adoption> Adoptions { get; set; }
    public DbSet<Sponsorship> Sponsorships { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<PixTransaction> PixTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new AnimalMap());
        modelBuilder.ApplyConfiguration(new DonationMap());
        modelBuilder.ApplyConfiguration(new AdoptionMap());
        modelBuilder.ApplyConfiguration(new SponsorshipMap());
        modelBuilder.ApplyConfiguration(new InstitutionMap());
        modelBuilder.ApplyConfiguration(new PixTransactionMap());
    }
}
