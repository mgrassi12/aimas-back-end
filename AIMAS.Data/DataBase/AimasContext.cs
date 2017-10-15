using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using AIMAS.Data.Util;
using AIMAS.Data.Models;

namespace AIMAS.Data
{
  public class AimasContext : IdentityDbContext<UserModel_DB, RoleModel_DB, long>
  {
    public DbSet<ChangeEventModel_DB> ChangeEvents { get; set; }

    public DbSet<InventoryModel_DB> Inventories { get; set; }

    public DbSet<InventoryAlertTimeModel_DB> InventoryAlertTimes { get; set; }

    public DbSet<LocationModel_DB> Locations { get; set; }

    public DbSet<ReservationModel_DB> Reservations { get; set; }

    public DbSet<ReportModel_DB> Reports { get; set; }

    public DbSet<ReservationInventoryModel_DB> ReservationInventories { get; set; }


    public AimasContext(DbContextOptions<AimasContext> options) : base(options)
    {
    }

    public InventoryModel_DB GetDbInventory(InventoryModel inventory)
    {
      if (inventory == null || inventory.ID == 0L)
        return null;
      return Inventories.Single(dbInventory => dbInventory.ID == inventory.ID);
    }

    public LocationModel_DB GetDbLocation(LocationModel location)
    {
      if (location == null || location.ID == 0L)
        return null;
      return Locations.Single(dbLocation => dbLocation.ID == location.ID);
    }

    public UserModel_DB GetDbUser(UserModel user)
    {
      if (user == null || user.Id == 0L)
        return null;
      return Users.Single(dbUser => dbUser.Id == user.Id);
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

      // Reservation Inventory Many to Many
      modelBuilder.Entity<ReservationInventoryModel_DB>()
            .HasKey(ri => new { ri.ReservationID, ri.InventoryID });

      modelBuilder.Entity<ReservationInventoryModel_DB>()
          .HasOne(ri => ri.Reservation)
          .WithMany(r => r.ReservationInventories)
          .HasForeignKey(ri => ri.ReservationID);

      modelBuilder.Entity<ReservationInventoryModel_DB>()
           .HasOne(ri => ri.Inventory)
           .WithMany(i => i.ReservationInventories)
           .HasForeignKey(ri => ri.InventoryID);

      base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
      var list = ChangeTracker
        .Entries()
        .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added && x.Entity != null)
        .Select(x => x.Entity)
        .ToList();

      list.ForEach(DateTimeKindAttribute.Apply);


      return base.SaveChanges();
    }

  }
}
