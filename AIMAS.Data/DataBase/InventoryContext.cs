using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  public class InventoryContext : DbContext
  {
    public DbSet<Inventory> Inventories { get; set; }

    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    public static void Initialize(InventoryContext context)
    {
      //context.Database.Migrate();

      if (!context.Inventories.Any())
      {
        // Add Test Inventory Rows
        context.Inventories.Add(new Inventory() { Name = "This is a Test Item #1" });
        context.Inventories.Add(new Inventory() { Name = "This is a Test Item #2" });
        context.Inventories.Add(new Inventory() { Name = "This is a Test Item #3" });
        context.SaveChanges();
      }
    }

  }

  [Table("inventory")]
  public class Inventory
  {
    [Key]
    public int ID { get; set; }
    [Required, Column(TypeName = "varchar(50)")]
    public string Name { get; set; }
  }
}
