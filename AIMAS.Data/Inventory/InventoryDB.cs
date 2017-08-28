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
      //context.Database.Migrate();

      if (!Aimas.Inventories.Any())
      {
        // Add Location
        var location = new LocationModel_DB("Test Location", id: 1);
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
                      Description = inventory.Location.Description
                    }
                  };
      return query.ToList();
    }

    public List<InventoryModel> GetInventories(InventorySearch search)
    {
      var query = from inventory in Aimas.Inventories
                  where inventory.Name.Contains(search.Name)
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
                      Description = inventory.Location.Description
                    }
                  };

      return query.ToList();
    }

  }
}
