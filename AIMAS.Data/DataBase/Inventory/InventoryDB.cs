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

    public AimasContext Aimas { get; }

    public InventoryDB(AimasContext Aimas)
    {
      this.Aimas = Aimas;
    }

    public void Initialize()
    {
      // Add Location
      var location = new LocationModel_DB("Test Location", id: 1, description: "Test");
      Aimas.Locations.Add(location);
      Aimas.SaveChanges();

      // Add Test Inventory Rows
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #1", DateTime.Now, DateTime.Now, location, id: 1));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #2", DateTime.Now, DateTime.Now, location, id: 2));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #3", DateTime.Now, DateTime.Now, location, id: 3));
      Aimas.SaveChanges();

      // Add Reservation
      Aimas.Reservations.Add(new ReservationModel_DB(Aimas.Users.First(), Aimas.Inventories.First(), DateTime.Now, DateTime.Now, 1));
      Aimas.SaveChanges();
    }

    public List<InventoryModel> GetInventories()
    {
      var query = from inventory in Aimas.Inventories
                  select new InventoryModel()
                  {
                    ID = inventory.ID,
                    Name = inventory.Name,
                    Description = inventory.Description,
                    ExpirationDate = inventory.ExpirationDate,
                    MaintanceDate = inventory.MaintanceDate,
                    Location = new LocationModel()
                    {
                      ID = inventory.Location.ID,
                      Name = inventory.Location.Name,
                      Description = inventory.Location.Description
                    }
                  };
      return query.ToList();
    }

    public (List<InventoryModel> list, int TotalCount) GetInventories(InventorySearch search)
    {
      var query = from inventory in Aimas.Inventories
                  select new InventoryModel()
                  {
                    ID = inventory.ID,
                    Name = inventory.Name,
                    Description = inventory.Description,
                    ExpirationDate = inventory.ExpirationDate,
                    MaintanceDate = inventory.MaintanceDate,
                    Location = new LocationModel()
                    {
                      ID = inventory.Location.ID,
                      Name = inventory.Location.Name,
                      Description = inventory.Location.Description
                    }
                  };

      if (search.ID.HasValue)
        query = query.Where(i => i.ID == search.ID.Value);
      if (!string.IsNullOrEmpty(search.Name))
        query = query.Where(i => i.Name.Contains(search.Name));
      if (!string.IsNullOrEmpty(search.Description))
        query = query.Where(i => i.Description.Contains(search.Description));

      var count = query.Count();
      query = query.Skip(search.PageSize * search.PageIndex);
      query = query.Take(search.PageSize);

      return (query.ToList(), count);
    }

    public void AddInventory(InventoryModel inventory)
    {
      var inventoryDB = inventory.ToDBModel();

      inventoryDB.Location = Aimas.Locations.Single(l => l.ID == inventoryDB.Location.ID);

      if (inventoryDB.ID != default)
      {
        var lastID = Aimas.Inventories.LastOrDefault()?.ID;
        var id = lastID.HasValue ? lastID.Value + 1 : 1;
        inventoryDB.ID = id;
      }

      Aimas.Inventories.Add(inventoryDB);
      Aimas.SaveChanges();
    }

    public void UpdateInventory(InventoryModel inventory)
    {
      var result = Aimas.Inventories.Single(i => i.ID == inventory.ID);

      if (!string.IsNullOrEmpty(inventory.Name))
        result.Name = inventory.Name;

      if (!string.IsNullOrEmpty(inventory.Description))
        result.Description = inventory.Description;

      if (inventory.ExpirationDate != default)
        result.ExpirationDate = inventory.ExpirationDate;

      if (inventory.MaintanceDate != default)
        result.MaintanceDate = inventory.MaintanceDate;

      if (inventory.Location.ID != result.Location.ID)
        result.Location = Aimas.Locations.Single(l => l.ID == inventory.ID);

      Aimas.SaveChanges();

    }

    public void RemoveInventory(int ID)
    {
      var item = new InventoryModel_DB { ID = ID };
      Aimas.Inventories.Attach(item);
      Aimas.Inventories.Remove(item);
      Aimas.SaveChanges();
    }

  }
}
