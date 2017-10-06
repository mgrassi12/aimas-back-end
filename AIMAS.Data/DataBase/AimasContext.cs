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
    public DbSet<AlertTimeModel_DB> AlertTimes { get; set; }

    public DbSet<AlertTimeInventoryModel_DB> AlertTimeInventories { get; set; }

    public DbSet<CategoryModel_DB> Categories { get; set; }

    public DbSet<CategoryInventoryModel_DB> CategoryInventories { get; set; }

    public DbSet<ChangeEventModel_DB> ChangeEvents { get; set; }

    public DbSet<InventoryModel_DB> Inventories { get; set; }

    public DbSet<LocationModel_DB> Locations { get; set; }

    public DbSet<NotificationModel_DB> Notifications { get; set; }

    public DbSet<ReservationModel_DB> Reservations { get; set; }

    public DbSet<ReportModel_DB> Reports { get; set; }

    public DbSet<ReservationInventoryModel_DB> ReservationInventories { get; set; }

    public DbSet<TimeLogModel_DB> TimeLogs { get; set; }


    public AimasContext(DbContextOptions<AimasContext> options) : base(options)
    {
    }

    public InventoryModel_DB GetDbInventory(InventoryModel inventory)
    {
      if (inventory == null)
        return null;
      return Inventories.Single(dbInventory => dbInventory.ID == inventory.ID);
    }

    public LocationModel_DB GetDbLocation(LocationModel location)
    {
      if (location == null)
        return null;
      return Locations.Single(dbLocation => dbLocation.ID == location.ID);
    }

    public UserModel_DB GetDbUser(UserModel user)
    {
      if (user == null)
        return null;
      return Users.Single(dbUser => dbUser.Id == user.Id);
    }

    public long GetNewIdFromLastId(long? lastId)
    {
      return lastId.HasValue ? lastId.Value + 1 : 1;
    }

    public long GetNewIdForInventory()
    {
      var lastId = Inventories.OrderBy(i => i.ID).LastOrDefault()?.ID;
      return GetNewIdFromLastId(lastId);
    }

    public long GetNewIdForTimeLog()
    {
      var lastId = TimeLogs.OrderBy(i => i.ID).LastOrDefault()?.ID;
      return GetNewIdFromLastId(lastId);
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

      // Catergory Inventory Many to Many
      modelBuilder.Entity<CategoryInventoryModel_DB>()
            .HasKey(ci => new { ci.CategoryID, ci.InventoryID });

      modelBuilder.Entity<CategoryInventoryModel_DB>()
          .HasOne(ci => ci.Category)
          .WithMany(c => c.CategoryInventories)
          .HasForeignKey(ci => ci.CategoryID);

      modelBuilder.Entity<CategoryInventoryModel_DB>()
           .HasOne(ci => ci.Inventory)
           .WithMany(i => i.CategoryInventories)
           .HasForeignKey(ci => ci.InventoryID);

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

      // Arlert Inventory One to Many Via Link Table
      modelBuilder.Entity<AlertTimeInventoryModel_DB>()
           .HasKey(ai => new { ai.AlertID, ai.InventoryID });

      modelBuilder.Entity<AlertTimeInventoryModel_DB>()
          .HasOne(ai => ai.AlertTime)
          .WithMany()
          .HasForeignKey(ai => ai.AlertID);

      modelBuilder.Entity<AlertTimeInventoryModel_DB>()
           .HasOne(ai => ai.Inventory)
           .WithMany(i => i.AlertTimeInventories)
           .HasForeignKey(ai => ai.InventoryID);

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
