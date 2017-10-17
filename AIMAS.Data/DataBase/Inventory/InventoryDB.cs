using System;
using System.Collections.Generic;
using System.Linq;
using AIMAS.Data.Identity;
using Microsoft.EntityFrameworkCore;
using AIMAS.Data.Models;

namespace AIMAS.Data.Inventory
{
  public class InventoryDB
  {

    private AimasContext Aimas { get; }

    public InventoryDB(AimasContext aimas)
    {
      Aimas = aimas;
    }

    public void Initialize()
    {
      // Add Location
      var location = new LocationModel_DB("Test Location", "Test");
      Aimas.Locations.Add(location);
      Aimas.SaveChanges();

      // Add Test Inventory Rows
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #1", DateTime.Now.AddDays(20), 10, location));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #2", DateTime.Now.AddDays(30), 20, location));
      Aimas.Inventories.Add(new InventoryModel_DB("Test Item #3", DateTime.Now.AddDays(40), 30, location));
      Aimas.SaveChanges();

      var inventory = Aimas.Inventories.First();
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 1));
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 5));
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 10));
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 20));
      inventory.AlertTimeInventories.Add(new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 30));
      Aimas.SaveChanges();

      // Add Reservation
      var reservation = new ReservationModel_DB(Aimas.Users.First(), DateTime.Now, DateTime.Now, "Test Purpose", location);
      reservation.ReservationInventories.Add(new ReservationInventoryModel_DB(reservation, inventory));
      Aimas.Reservations.Add(reservation);
      Aimas.SaveChanges();
    }

    #region InventoryOperations

    private static IQueryable<InventoryModel_DB> IncludeOtherModels_Inventory(IQueryable<InventoryModel_DB> query)
    {
      return query.Include(x => x.CurrentLocation)
        .Include(x => x.DefaultLocation)
        .Include(x => x.AlertTimeInventories);
    }

    public void AddInventory(InventoryModel inventory)
    {
      var inventoryDb = inventory.CreateNewDbModel(Aimas);
      Aimas.Inventories.Add(inventoryDb);
      Aimas.SaveChanges();
    }

    public List<InventoryModel> GetInventories()
    {
      var query = IncludeOtherModels_Inventory(Aimas.Inventories)
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

      var finalQuery = IncludeOtherModels_Inventory(query)
        .Select(i => i.ToModel());

      return (finalQuery.ToList(), count);
    }

    public List<InventoryModel> GetExpiredInventory()
    {
      var query = from inventory in Aimas.Inventories.Include(x => x.CurrentLocation)
                  where inventory.ExpirationDate <= DateTime.UtcNow
                  where (from report in inventory.Reports where report.Type == ReportType.ExpirationDisposal select report).Count() == 0
                  select inventory.ToModel();
      return query.ToList();
    }

    public List<InventoryModel> GetInventoryNeedingMaintenance()
    {
      var query = from inventory in Aimas.Inventories.Include(x => x.CurrentLocation)
                  let MaintenanceDate = (from report in inventory.Reports where report.Type == ReportType.Maintenance orderby report.ExecutionDate descending select report.ExecutionDate).FirstOrDefault()
                  where (MaintenanceDate != null && MaintenanceDate <= DateTime.UtcNow || MaintenanceDate == null && inventory.CreationDate <= DateTime.UtcNow)
                  where (from report in inventory.Reports where report.Type == ReportType.ExpirationDisposal select report).Count() == 0
                  select inventory.ToModel();
      return query.ToList();
    }

    public List<InventoryModel> GetCriticalInventoryNotInDefaultLocation()
    {
      var query = from inventory in Aimas.Inventories
                  where inventory.IsCritical
                  where inventory.DefaultLocation != null
                  where inventory.CurrentLocation.ID != inventory.DefaultLocation.ID
                  select inventory.ToModel();
      return query.ToList();
    }

    public void UpdateInventory(InventoryModel inventory, UserModel_DB changeUser)
    {
      var result = IncludeOtherModels_Inventory(Aimas.Inventories)
        .First(item => item.ID == inventory.ID);
      result.UpdateDb(inventory, changeUser, Aimas);
      Aimas.SaveChanges();
    }

    public void RemoveInventory(long id)
    {
      var item = Aimas.Inventories.Find(id);
      Aimas.Inventories.Remove(item);
      Aimas.SaveChanges();
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

    private IQueryable<InventoryAlertTimeModel_DB> GetUpcomingAlertTimeQuery(InventoryAlertTimeType type, Func<InventoryModel_DB, DateTime> getDate)
    {
      var query = from alert in Aimas.InventoryAlertTimes
                  where alert.Inventory.ExpirationDate >= DateTime.UtcNow
                  && !alert.Inventory.IsDisposed()
                  && alert.Type == type
                  && !alert.SentTime.HasValue
                  && (getDate(alert.Inventory) - DateTime.Now) <= TimeSpan.FromDays(alert.DaysBefore)
                  select alert;
      return query.Include(alert => alert.Inventory).Include(alert => alert.Inventory.Reports);
    }

    public List<InventoryAlertTimeModel_DB> GetUpcomingExpiryAlertTimes()
    {
      return GetUpcomingAlertTimeQuery(InventoryAlertTimeType.Inventory_E_Date, i => i.ExpirationDate).ToList();
    }

    public List<InventoryAlertTimeModel_DB> GetUpcomingMaintenanceAlertTimes()
    {
      return GetUpcomingAlertTimeQuery(InventoryAlertTimeType.Inventory_M_Date, i => i.GetMaintenanceDate()).ToList();
    }

    public void RemoveInventoryAlertTime(long id)
    {
      var item = Aimas.InventoryAlertTimes.Find(id);
      Aimas.InventoryAlertTimes.Remove(item);
      Aimas.SaveChanges();
    }
    #endregion

    #region LocationOperations

    public void AddLocation(LocationModel location)
    {
      var locationDb = location.CreateNewDbModel(Aimas);
      Aimas.Locations.Add(locationDb);
      Aimas.SaveChanges();
    }

    public List<LocationModel> GetLocations()
    {
      var query = from location in Aimas.Locations
                  select location.ToModel();
      return query.ToList();
    }

    public (List<LocationModel> list, int TotalCount) GetLocations(LocationSearch search)
    {
      var query = Aimas.Locations.AsQueryable();

      if (!string.IsNullOrEmpty(search.Name))
        query = query.Where(l => l.Name.ToLower().Contains(search.Name.ToLower()));
      if (!string.IsNullOrEmpty(search.Description))
        query = query.Where(l => l.Description.ToLower().Contains(search.Description.ToLower()));

      var count = query.Count();
      query = query.Skip(search.PageSize * search.PageIndex);
      query = query.Take(search.PageSize);

      var finalQuery = query.Select(i => i.ToModel());
      return (finalQuery.ToList(), count);
    }

    public void UpdateLocation(LocationModel location)
    {
      var result = Aimas.Locations
        .First(dbLocation => dbLocation.ID == location.ID);
      result.UpdateDb(location);
      Aimas.SaveChanges();
    }

    public void RemoveLocation(long id)
    {
      var location = Aimas.Locations.Find(id);
      Aimas.Locations.Remove(location);
      Aimas.SaveChanges();
    }

    #endregion

    #region ReportModelOperations

    private static IQueryable<ReportModel_DB> IncludeOtherModels_Report(IQueryable<ReportModel_DB> query)
    {
      return query.Include(r => r.Inventory)
        .Include(r => r.Creator)
        .Include(r => r.Executor);
    }

    public void AddReport(ReportModel report, UserModel_DB user)
    {
      report.Creator = new UserModel(user.Id);
      report.CreationDate = DateTime.Now;
      if (report.ExecutionDate == null) report.ExecutionDate = report.CreationDate;
      var reportDb = report.CreateNewDbModel(Aimas);
      Aimas.Reports.Add(reportDb);
      Aimas.SaveChanges();
    }

    public List<ReportModel> GetReports()
    {
      var query = IncludeOtherModels_Report(Aimas.Reports)
        .Select(r => r.ToModel());
      return query.ToList();
    }

    public List<ReportModel> GetReportsForInventory(long inventoryId)
    {
      var query = IncludeOtherModels_Report(Aimas.Reports)
        .Where(r => r.Inventory.ID == inventoryId)
        .Select(r => r.ToModel());
      return query.ToList();
    }

    public (List<ReportModel> list, int TotalCount) GetReports(ReportSearch search)
    {
      var query = Aimas.Reports.AsQueryable();

      if (!String.IsNullOrEmpty(search.InventoryName))
        query = query.Where(i => i.Inventory.Name.ToLower().Contains(search.InventoryName.ToLower()));
      if (!String.IsNullOrEmpty(search.UserName))
        query = query.Where(i =>
          i.Creator.FirstName.ToLower().Contains(search.UserName.ToLower())
          ||
          i.Creator.LastName.ToLower().Contains(search.UserName.ToLower())
          ||
          i.Executor.FirstName.ToLower().Contains(search.UserName.ToLower())
          ||
          i.Executor.LastName.ToLower().Contains(search.UserName.ToLower())
        );
      if (search.Type.HasValue)
        query = query.Where(i => i.Type == search.Type.Value);

      var count = query.Count();
      query = query.Skip(search.PageSize * search.PageIndex);
      query = query.Take(search.PageSize);

      var finalQuery = IncludeOtherModels_Report(query)
        .Select(i => i.ToModel());

      return (finalQuery.ToList(), count);
    }

    public void RemoveReport(long id)
    {
      var report = Aimas.Reports.Find(id);
      Aimas.Reports.Remove(report);
      Aimas.SaveChanges();
    }

    #endregion

    #region ReservationOperations
    public void AddReservation(ReservationModel reservation, UserModel_DB user)
    {
      var res = reservation.CreateNewDbModel(Aimas);
      res.User = user;
      Aimas.Reservations.Add(res);
      Aimas.SaveChanges();
    }

    public List<ReservationModel> GetReservations()
    {
      var query = from reservation in Aimas.Reservations
                  select reservation.ToModel();
      return query.ToList();
    }

    public (List<ReservationModel> list, int TotalCount) GetReservations(ReservationSearch search)
    {
      var query = Aimas.Reservations.AsQueryable();

      if (!string.IsNullOrEmpty(search.UserName))
        query = query.Where(i => i.User.FirstName.ToLower().Contains(search.UserName.ToLower()) || i.User.LastName.ToLower().Contains(search.UserName.ToLower()));

      var count = query.Count();
      query = query.Skip(search.PageSize * search.PageIndex);
      query = query.Take(search.PageSize);

      var finalQuery = query
        .Include(x => x.User)
        .Include(x => x.Location)
        .Include(x => x.ReservationInventories)
        .ThenInclude(x => x.Inventory)
        .Select(i => i.ToModel());

      return (finalQuery.ToList(), count);
    }

    public void UpdateReservation(ReservationModel reservation)
    {
      var result = Aimas.Reservations
        .Include(x => x.Location)
        .Include(x => x.ReservationInventories)
        .First(r => r.ID == reservation.ID);
      result.UpdateDb(reservation, Aimas);
      Aimas.SaveChanges();
    }

    public void RemoveReservation(long id)
    {
      var reservation = Aimas.Reservations.Find(id);
      Aimas.Reservations.Remove(reservation);
      Aimas.SaveChanges();
    }

    #endregion

    #region ReservationInventoryOperations

    public void AddInventoryToReservation(long reservationId, long inventoryId)
    {
      var reservation = Aimas.Reservations.Find(reservationId);
      var inventory = Aimas.Inventories.Find(inventoryId);
      Aimas.ReservationInventories.Add(new ReservationInventoryModel_DB(reservation, inventory));
      Aimas.SaveChanges();
    }

    public List<ReservationModel> GetReservationsForInventory(long inventoryId)
    {
      var query = from inventory in Aimas.Inventories
                  from reservationInventory in Aimas.ReservationInventories
                  where inventory.ID == inventoryId
                  select reservationInventory.Reservation.ToModel();
      return query.ToList();
    }

    public List<InventoryModel> GetItemsForReservation(long reservationId)
    {
      var query = from reservation in Aimas.Reservations
                  from reservationInventory in Aimas.ReservationInventories
                  where reservation.ID == reservationId
                  select reservationInventory.Inventory.ToModel();
      return query.ToList();
    }

    public void RemoveInventoryFromReservation(long reservationId, long inventoryId)
    {
      var entry = Aimas.ReservationInventories.Find(reservationId, inventoryId);
      Aimas.ReservationInventories.Remove(entry);
      Aimas.SaveChanges();
    }

    #endregion
  }
}

