using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AIMAS.Data.Models;

namespace AIMAS.Data.Inventory
{
  public class InventoryDB
  {

    public InventoryContext Inventory { get; }

    public InventoryDB(InventoryContext inventoryContext)
    {
      Inventory = inventoryContext;
    }

    public void Initialize()
    {
      //context.Database.Migrate();

      if (!Inventory.Inventories.Any())
      {
        // Add Test Inventory Rows
        Inventory.Inventories.Add(new InventoryModel_DB() { Name = "This is a Test Item #1" });
        Inventory.Inventories.Add(new InventoryModel_DB() { Name = "This is a Test Item #2" });
        Inventory.Inventories.Add(new InventoryModel_DB() { Name = "This is a Test Item #3" });
        Inventory.SaveChanges();
      }
    }

    public List<InventoryModel> GetInventories()
    {
      var list = from inventory in Inventory.Inventories
                 select new InventoryModel()
                 {
                   Name = inventory.Name
                 };
      return list.ToList();
    }

  }
}
