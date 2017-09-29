using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using AIMAS.Data.Util;

namespace AIMAS.Data
{
  public class AimasContext : IdentityDbContext<UserModel_DB, RoleModel_DB, long>
  {

    public DbSet<InventoryModel_DB> Inventories { get; set; }

    public DbSet<ReservationModel_DB> Reservations { get; set; }

    public DbSet<LocationModel_DB> Locations { get; set; }


    public AimasContext(DbContextOptions<AimasContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

      modelBuilder.Entity<UserModel_DB>()
        .HasAlternateKey(user => user.UserName)
        .HasName("AK_AspNetUsers_UserName");

      modelBuilder.Entity<UserModel_DB>()
        .HasAlternateKey(user => user.Email)
        .HasName("AK_AspNetUsers_Email");

      modelBuilder.Entity<RoleModel_DB>()
        .HasAlternateKey(role => role.Name)
        .HasName("AK_AspNetRoles_Name");

      base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
      var list = ChangeTracker
        .Entries()
        .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added && x.Entity != null)
        .Select(x => x.Entity)
        .ToList();

        list.ForEach(entity => DateTimeKindAttribute.Apply(entity));


      return base.SaveChanges();
    }

  }
}
