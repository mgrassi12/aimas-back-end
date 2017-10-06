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
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #1", DateTime.Now.AddDays(20), 10, location, location));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #2", DateTime.Now.AddDays(30), 20, location, location));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #3", DateTime.Now.AddDays(40), 30, location, location));
      Aimas.SaveChanges();

      // Add Alerts
      Aimas.AlertTimes.Add(new AlertTimeModel_DB(1, "Dayly"));
      Aimas.AlertTimes.Add(new AlertTimeModel_DB(7, "Weekly"));
      Aimas.AlertTimes.Add(new AlertTimeModel_DB(14, "Fortnightly"));
      Aimas.AlertTimes.Add(new AlertTimeModel_DB(30, "Monthly"));
      Aimas.SaveChanges();

      var inventory = Aimas.Inventories.First();
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(1), inventory, AlertTimeType.Inventoy_E_Date));
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(5), inventory, AlertTimeType.Inventoy_E_Date));
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(10), inventory, AlertTimeType.Inventoy_E_Date));
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(20), inventory, AlertTimeType.Inventoy_E_Date));
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(30), inventory, AlertTimeType.Inventoy_E_Date));
      Aimas.SaveChanges();

      // Add Reservation
      var reservation = new ReservationModel_DB(Aimas.Users.First(), DateTime.Now, DateTime.Now, "Test Purpose", location);
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
      inventoryDB.CurrentLocation = Aimas.GetDbLocation(inventory.CurrentLocation);
      inventoryDB.DefaultLocation = Aimas.GetDbLocation(inventory.DefaultLocation);

      if (inventoryDB.ID == default)
        inventoryDB.ID = Aimas.GetNewIdForInventory();

      Aimas.Inventories.Add(inventoryDB);
      Aimas.SaveChanges();
    }

    public void UpdateInventory(InventoryModel inventory)
    {
      var result = Aimas.Inventories
        .Include(item => item.CurrentLocation)
        .Where(item => item.ID == inventory.ID)
        .First();
      result.UpdateDb(inventory, Aimas);
      Aimas.SaveChanges();
    }

    public void RemoveInventory(long ID)
    {
      var item = Aimas.Inventories.Find(ID);
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

    public List<AlertTimeInventoryModel_DB> GetUpcomingExpiryAlertTimes()
    {
      //var t = aimas.alerttimes.include(i => i.inventory).tolist();
      //var s = t.where(x => x.type == alerttimetype.inventoy_e_date).tolist();
      //var d = s.where(x => (x.inventory.expirationdate - datetime.now) >= timespan.fromdays(x.daysbefore)).tolist();
      var query = from alertTimeInventory in Aimas.AlertTimeInventories
                  where
                    alertTimeInventory.Inventory.ExpirationDate >= DateTime.UtcNow
                    &&
                    alertTimeInventory.Type == AlertTimeType.Inventoy_E_Date
                    &&
                    (alertTimeInventory.Inventory.ExpirationDate - DateTime.Now) <= TimeSpan.FromDays(alertTimeInventory.AlertTime.DaysBefore)
                  group alertTimeInventory by alertTimeInventory.Inventory.ID into g
                  select new { inventoyID = g.Key, minDay = g.Min(x => x.AlertTime.DaysBefore) };

      var query2 = from q in query
                   join alertTimeInventory in Aimas.AlertTimeInventories on q.inventoyID equals alertTimeInventory.InventoryID
                   where alertTimeInventory.AlertTime.DaysBefore == q.minDay
                   select alertTimeInventory;

      return query2.Include(x => x.AlertTime).Include(x => x.Inventory).ToList();
    }

    public void AddTimeLog(TimeLogModel log)
    {
      //var dbLog = log.ToDbModel();
      //dbLog.User = Aimas.GetDbUser(log.User);

      //if (dbLog.ID == default)
      //  dbLog.ID = Aimas.GetNewIdForTimeLog();

      //Aimas.TimeLogs.Add(dbLog);
      //Aimas.SaveChanges();
    }

    public List<AlertTimeModel> GetAlertTimes()
    {
      var query = from alertTime in Aimas.AlertTimes
                  select alertTime.ToModel();
      return query.ToList();
    }

    public List<AlertTimeModel> GetAlertTimes(InventoryModel inventory)
    {
      var query = from alertTime in Aimas.AlertTimes
                  select alertTime.ToModel();
      return query.ToList();
    }

    public void UpdateAlertTime(AlertTimeModel alertTime)
    {
      var result = Aimas.AlertTimes
        .Include(dbAlertTime => dbAlertTime.Inventory)
        .Where(dbAlertTime => dbAlertTime.ID == alertTime.ID)
        .First();
      result.UpdateDb(alertTime, Aimas);
      Aimas.SaveChanges();
    }

    public void RemoveAlertTime(int id)
    {
      var alertTime = Aimas.AlertTimes.Find((long)id);
      Aimas.AlertTimes.Remove(alertTime);
      Aimas.SaveChanges();
    }


    //TODO: Create CRUD methods for all inventory classes
  }
}

