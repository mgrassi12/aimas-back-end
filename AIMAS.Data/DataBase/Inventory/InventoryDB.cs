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

      var inventory = Aimas.Inventories.First();
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(AlertInventoryTimeType.Inventory_E_Date, 1));
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(AlertInventoryTimeType.Inventory_E_Date, 5));
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(AlertInventoryTimeType.Inventory_E_Date, 10));
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(AlertInventoryTimeType.Inventory_E_Date, 20));
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(AlertInventoryTimeType.Inventory_E_Date, 30));
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
      var inventoryDB = inventory.CreateNewDbModel(Aimas);

      //TODO: Is this ID setting needed? Are IDs automatically incremented if ID is default?
      //if (inventoryDB.ID == default)
      //  inventoryDB.ID = Aimas.GetNewIdForInventory();

      Aimas.Inventories.Add(inventoryDB);
      Aimas.SaveChanges();
    }

    public List<InventoryModel> GetInventories()
    {
      var query = Aimas.Inventories
        .Include(x => x.CurrentLocation)
        .Include(x => x.DefaultLocation)
        .Include(x => x.AlertTimeInventories)
        .Select(i => i.ToModel());
      return query.ToList();
    }

    public (List<InventoryModel> list, int TotalCount) GetInventories(InventorySearch search)
    {
      var query = Aimas.Inventories.AsQueryable();

      if (search.ID.HasValue && search.ID.Value != default)
        query = query.Where(i => i.ID == search.ID.Value);
      if (!string.IsNullOrEmpty(search.Name))
        query = query.Where(i => i.Name.ToLower().Contains(search.Name.ToLower()));
      if (!string.IsNullOrEmpty(search.Description))
        query = query.Where(i => i.Description.ToLower().Contains(search.Description.ToLower()));

      var count = query.Count();
      query = query.Skip(search.PageSize * search.PageIndex);
      query = query.Take(search.PageSize);

      var finalQuery = query
        .Include(x => x.CurrentLocation)
        .Include(x => x.DefaultLocation)
        .Include(x => x.AlertTimeInventories)
        .Select(i => i.ToModel());

      return (finalQuery.ToList(), count);
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
        .Include(item => item.AlertTimeInventories)
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

    #region LocationOperations

    public List<LocationModel> GetLocations()
    {
      var query = from location in Aimas.Locations
                  select location.ToModel();
      return query.ToList();
    }

    #endregion

    #region AlertTimeOperations

    public void AddInventoryAlertTime(InventoryAlertTimeModel alert)
    {
      Aimas.InventoryAlertTimes.Add(alert.CreateNewDbModel(Aimas));
      Aimas.SaveChanges();
    }

    public List<InventoryAlertTimeModel> GetInventoryAlertTimes(long id)
    {
      var query = from alert in Aimas.InventoryAlertTimes
                  where alert.Inventory.ID == id
                  select alert.ToModel();
      return query.ToList();
    }

    private IQueryable<InventoryAlertTimeModel_DB> GetUpcomingAlertTimeQuery(AlertInventoryTimeType type, Func<InventoryModel_DB, DateTime> getDate)
    {
      var query = from alert in Aimas.InventoryAlertTimes
                  where alert.Inventory.ExpirationDate >= DateTime.UtcNow
                  && alert.Type == type
                  && !alert.SentTime.HasValue
                  && (getDate(alert.Inventory) - DateTime.Now) <= TimeSpan.FromDays(alert.DaysBefore)
                  select alert;
      return query.Include(alert => alert.Inventory);
    }

    public List<InventoryAlertTimeModel_DB> GetUpcomingExpiryAlertTimes()
    {
      var query = GetUpcomingAlertTimeQuery(AlertInventoryTimeType.Inventory_E_Date, (i) => i.ExpirationDate);
      return query.ToList();
    }

    public void RemoveInventoryAlertTime(long ID)
    {
      var item = Aimas.InventoryAlertTimes.Find(ID);
      Aimas.InventoryAlertTimes.Remove(item);
      Aimas.SaveChanges();
    }
    #endregion

    #region ReservationOperations
    public void Addreservation(ReservationModel reservation)
    {
      Aimas.Reservations.Add(reservation.CreateNewDbModel(Aimas));
      Aimas.SaveChanges();
    }

    public List<ReservationModel> GetReservations()
    {
      var query = from reservation in Aimas.Reservations
                  select reservation.ToModel();
      return query.ToList();
    }

    public List<ReservationModel> GetReservationsForInventory(long inventoryId)
    {
      var query = from inventory in Aimas.Inventories
                  from reservationInventory in Aimas.ReservationInventories
                  where inventory.ID == inventoryId
                  select reservationInventory.Reservation.ToModel();
      return query.ToList();
    }

    public void UpdateReservation(ReservationModel reservation)
    {
      var result = Aimas.Reservations
        .Include(dbReservation => dbReservation.Location)
        .Where(dbReservation => dbReservation.ID == reservation.ID)
        .First();
      result.UpdateDb(reservation, Aimas);
      Aimas.SaveChanges();
    }

    public void RemoveReservation(long ID)
    {
      var item = Aimas.Reservations.Find(ID);
      Aimas.Reservations.Remove(item);
      Aimas.SaveChanges();
    }

    #endregion

    //TODO: Create CRUD methods for all inventory classes
  }
}

