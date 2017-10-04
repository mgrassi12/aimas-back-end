using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
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
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #1", DateTime.Now, DateTime.Now, location, location, id: 1));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #2", DateTime.Now, DateTime.Now, location, location, id: 2));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #3", DateTime.Now, DateTime.Now, location, location, id: 3));
      Aimas.SaveChanges();

      // Add Alerts
      var inventory = Aimas.Inventories.First();
      inventory.AlertTimes.Add(new AlertTimeModel_DB(inventory, AlertTimeType.Inventoy_E_Date, 30));
      inventory.AlertTimes.Add(new AlertTimeModel_DB(inventory, AlertTimeType.Inventoy_E_Date, 20));
      inventory.AlertTimes.Add(new AlertTimeModel_DB(inventory, AlertTimeType.Inventoy_E_Date, 10));
      inventory.AlertTimes.Add(new AlertTimeModel_DB(inventory, AlertTimeType.Inventoy_E_Date, 5));
      inventory.AlertTimes.Add(new AlertTimeModel_DB(inventory, AlertTimeType.Inventoy_E_Date, 1));
      Aimas.SaveChanges();

      // Add Reservation
      var reservation = new ReservationModel_DB(Aimas.Users.First(), DateTime.Now, DateTime.Now, "Test Purpose", location, id: 1);
      reservation.ReservationInventories.Add(new ReservationInventoryModel_DB(reservation, inventory));
      Aimas.Reservations.Add(reservation);
      Aimas.SaveChanges();

      // Add Category
      var cateogory = new CategoryModel_DB("Test Item");
      cateogory.CategoryInventories.Add(new CategoryInventoryModel_DB(cateogory, inventory));
      Aimas.Categories.Add(cateogory);
      Aimas.SaveChanges();
    }

    public List<InventoryModel> GetInventories()
    {
      var query = from inventory in Aimas.Inventories
                  select inventory.ToModel();
      return query.ToList();
    }

    public (List<InventoryModel> list, int TotalCount) GetInventories(InventorySearch search)
    {
      var query = from inventory in Aimas.Inventories
                  select inventory.ToModel();

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
      var inventoryDB = inventory.ToDbModel();

      inventoryDB.CurrentLocation = Aimas.Locations.Single(l => l.ID == inventoryDB.CurrentLocation.ID);

      if (inventoryDB.ID == default)
      {
        var lastID = Aimas.Inventories.OrderBy(i => i.ID).LastOrDefault()?.ID;
        var id = lastID.HasValue ? lastID.Value + 1 : 1;
        inventoryDB.ID = id;
      }

      Aimas.Inventories.Add(inventoryDB);
      Aimas.SaveChanges();
    }

    public void UpdateInventory(InventoryModel inventory)
    {
      var result = Aimas.Inventories
        .Include(item => item.CurrentLocation)
        .Where(item => item.ID == inventory.ID)
        .First();

      if (!string.IsNullOrEmpty(inventory.Name))
        result.Name = inventory.Name;

      if (!string.IsNullOrEmpty(inventory.Description))
        result.Description = inventory.Description;

      if (inventory.ExpirationDate != default)
        result.ExpirationDate = inventory.ExpirationDate;

      if (inventory.MaintenanceDate != default)
        result.MaintenanceDate = inventory.MaintenanceDate;

      if (inventory.CurrentLocation.ID != result.CurrentLocation.ID)
        result.CurrentLocation = Aimas.Locations.Single(l => l.ID == inventory.ID);

      Aimas.SaveChanges();

    }

    public void RemoveInventory(int ID)
    {
      var item = Aimas.Inventories.Find((long)ID);
      Aimas.Inventories.Remove(item);
      Aimas.SaveChanges();
    }

    public List<InventoryModel> GetExpiredInventory()
    {
      var query = from inventory in Aimas.Inventories
                  where inventory.ExpirationDate <= DateTime.UtcNow
                  select inventory.ToModel();
      return query.ToList();
    }

    public List<AlertTimeModel> GetUpcomingExpiryAlertTimes()
    {
      var query = from alert in Aimas.AlertTimes
                  where
                    alert.Type == AlertTimeType.Inventoy_E_Date
                    &&
                    alert.Inventory.ExpirationDate >= DateTime.UtcNow
                    &&
                    (alert.Inventory.ExpirationDate - DateTime.UtcNow) >= TimeSpan.FromDays(alert.DaysBefore)
                  select alert.ToModel();
      return query.ToList();
    }
  }


}

