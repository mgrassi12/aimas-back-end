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
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #1", DateTime.Now.AddDays(20), 10, location));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #2", DateTime.Now.AddDays(30), 20, location));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #3", DateTime.Now.AddDays(40), 30, location));
      Aimas.SaveChanges();

      // Add Alerts
      Aimas.AlertTimes.Add(new AlertTimeModel_DB(1, "One day before"));
      Aimas.AlertTimes.Add(new AlertTimeModel_DB(7, "One week before"));
      Aimas.AlertTimes.Add(new AlertTimeModel_DB(14, "One fortnight before"));
      Aimas.AlertTimes.Add(new AlertTimeModel_DB(30, "One month before"));
      Aimas.SaveChanges();

      var inventory = Aimas.Inventories.First();
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(1), inventory, AlertTimeType.Inventory_E_Date));
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(5), inventory, AlertTimeType.Inventory_E_Date));
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(10), inventory, AlertTimeType.Inventory_E_Date));
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(20), inventory, AlertTimeType.Inventory_E_Date));
      inventory.AlertTimeInventories.Add(new AlertTimeInventoryModel_DB(new AlertTimeModel_DB(30), inventory, AlertTimeType.Inventory_E_Date));
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

    #region InventoryOperations
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
      query.Include(x => x.CurrentLocation).Include(x => x.DefaultLocation);
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

    public List<InventoryModel> GetExpiredInventory()
    {
      var query = from inventory in Aimas.Inventories
                  where inventory.ExpirationDate <= DateTime.UtcNow
                  select inventory.ToModel();
      return query.ToList();
    }

    public void UpdateInventory(InventoryModel inventory)
    {
      var result = Aimas.Inventories
        .Include(item => item.CurrentLocation)
        .Include(item => item.DefaultLocation)
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
    #endregion

    private IQueryable<AlertTimeInventoryModel_DB> GetUpcomingAlertTimeQuery(AlertTimeType type, Func<InventoryModel_DB, DateTime> getDate)
    {
      var query = from alertTimeInventory in Aimas.AlertTimeInventories
                  where alertTimeInventory.Inventory.ExpirationDate >= DateTime.UtcNow
                  && alertTimeInventory.Type == type
                  && !alertTimeInventory.SentTime.HasValue
                  && (getDate(alertTimeInventory.Inventory) - DateTime.Now) <= TimeSpan.FromDays(alertTimeInventory.AlertTime.DaysBefore)
                  select alertTimeInventory;
      return query;
    }

    public List<AlertTimeInventoryModel_DB> GetUpcomingExpiryAlertTimes()
    {
      var query = GetUpcomingAlertTimeQuery(AlertTimeType.Inventory_E_Date, (i) => i.ExpirationDate);
      return query.Include(x => x.AlertTime).Include(x => x.Inventory).ToList();
    }

    //TODO: Remove Alert Time model - Move DaysBefore property to AlertTimeInventory model
    //TODO: In front end, give option of number of days (num * 1), weeks (num * 7) and months (num * 30) before
    #region AlertTimeOperations
    public List<AlertTimeModel> GetAlertTimes()
    {
      var query = from alertTime in Aimas.AlertTimes
                  select alertTime.ToModel();
      return query.ToList();
    }

    public List<AlertTimeModel> GetAlertTimes(long inventoryID)
    {
      var query = from i in Aimas.Inventories
                  from alertTimeInventory in i.AlertTimeInventories
                  where i.ID == inventoryID
                  select alertTimeInventory.AlertTime.ToModel();
      return query.ToList();
    }

    public void UpdateAlertTime(AlertTimeModel alertTime)
    {
      var result = Aimas.AlertTimes
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
    #endregion

    //TODO: Create CRUD methods for all inventory classes
  }
}

