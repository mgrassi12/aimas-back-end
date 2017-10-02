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
    public DbSet<AlertTimeModel_DB> AlertTimes { get; set; }

    public DbSet<CategoryModel_DB> Categories { get; set; }

    public DbSet<CategoryEntryModel_DB> CategoryEntries { get; set; }

    public DbSet<ChangeEventModel_DB> ChangeEvents { get; set; }

    public DbSet<InventoryModel_DB> Inventories { get; set; }

    public DbSet<LocationModel_DB> Locations { get; set; }

    public DbSet<NotificationModel_DB> Notifications { get; set; }

    public DbSet<ReservationModel_DB> Reservations { get; set; }

    public DbSet<ReportModel_DB> Reports { get; set; }

    public DbSet<ReservationEntryModel_DB> ReservationEntries { get; set; }

    public DbSet<TimeLogModel_DB> TimeLogs { get; set; }


    public AimasContext(DbContextOptions<AimasContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //modelBuilder.Entity<InventoryModel_DB>()
      //  .HasOne(inventory => inventory.CurrentLocation)
      //  .WithMany()
      //  .OnDelete(DeleteBehavior.Restrict);

      //modelBuilder.Entity<InventoryModel_DB>()
      //  .HasOne(inventory => inventory.DefaultLocation)
      //  .WithMany()
      //  .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

      modelBuilder.Entity<UserModel_DB>()
        .HasAlternateKey(user => user.UserName)
        .HasName("AK_AspNetUsers_UserName");

      modelBuilder.Entity<UserModel_DB>()
        .HasAlternateKey(user => user.Email)
        .HasName("AK_AspNetUsers_Email");

      modelBuilder.Entity<RoleModel_DB>()
        .HasAlternateKey(role => role.Name)
        .HasName("AK_AspNetRoles_Name");

      modelBuilder.Entity<CategoryModel_DB>()
        .HasAlternateKey(category => category.Name)
        .HasName("AK_category_Name");

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
