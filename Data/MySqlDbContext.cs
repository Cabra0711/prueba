using Microsoft.EntityFrameworkCore;
using PoliRiwi.Models;

namespace PoliRiwi.Data;

public class MySqlDbContext : DbContext
{
    // we use FLUENTAPI to define our FK and relations inside of the database to check if the relations exits before 
    // enter data to the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservations>().HasOne(p => p.Place).WithMany(p => p.Reservations).HasForeignKey(p => p.PlaceId);
        modelBuilder.Entity<Reservations>().HasOne(u => u.User).WithMany(p => p.Reservations).HasForeignKey(p => p.UserId);
    }
    
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
    {
        
    }
    public DbSet<Places> Places { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Reservations> Reservations { get; set; }
}


