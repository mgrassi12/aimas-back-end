using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using AIMAS.Data.Models;
using AIMAS.Data.Util;

namespace AIMAS.Data.Inventory
{
  [Table("inventory")]
  public class InventoryModel_DB : IAimasDbModelWithUpdate<InventoryModel>
  {
    [Key]
    public long ID { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime CreationDate { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime ExpirationDate { get; set; }

    public long MaintenanceIntervalDays { get; set; }

    [Required]
    public LocationModel_DB CurrentLocation { get; set; }

    public LocationModel_DB DefaultLocation { get; set; }

    public List<InventoryAlertTimeModel_DB> AlertTimeInventories { get; set; }

    public List<ReportModel_DB> Reports { get; set; }

    public List<ReservationInventoryModel_DB> ReservationInventories { get; set; }

    [Required]
    public bool IsCritical { get; set; }

    private InventoryModel_DB()
    {

    }

    public InventoryModel_DB(
      string name,
      DateTime expire,
      long intervalDays,
      LocationModel_DB currentLocation,
      LocationModel_DB defaultLocation = default,
      string description = default,
      bool isCritical = default,
      List<InventoryAlertTimeModel_DB> alertTimeInventories = default,
      long id = default
      )
    {
      ID = id;
      Name = name;
      Description = description;
      CreationDate = DateTime.UtcNow;
      ExpirationDate = expire;
      MaintenanceIntervalDays = intervalDays;
      CurrentLocation = currentLocation;
      DefaultLocation = defaultLocation;
      IsCritical = isCritical;
      AlertTimeInventories = alertTimeInventories ?? new List<InventoryAlertTimeModel_DB>();
      Reports = new List<ReportModel_DB>();
      ReservationInventories = new List<ReservationInventoryModel_DB>();
    }

    public DateTime GetMaintenanceDate()
    {
      var reports = Reports.Where(r => r.Type == ReportType.Maintenance).ToList();
      var lastMaintenance = CreationDate;
      if (reports.Any())
      {
        var report = reports.OrderByDescending(r => r.ExecutionDate).First();
        lastMaintenance = report.ExecutionDate;
      }
      return lastMaintenance.AddDays(MaintenanceIntervalDays);
    }

    public bool IsAvailable()
    {
      return ExpirationDate >= DateTime.UtcNow
        && GetMaintenanceDate() >= DateTime.UtcNow
        && !IsDisposed();
      //TODO: ADD RESERVATION STUFF AS WELL
    }

    public bool IsDisposed()
    {
      return Reports.Exists(r => r.Type == ReportType.ExpirationDisposal);
    }

    public InventoryModel ToModel()
    {
      return new InventoryModel(
        id: ID,
        name: Name,
        description: Description,
        expiration: ExpirationDate,
        intervalDays: MaintenanceIntervalDays,
        currentLocation: CurrentLocation?.ToModel(),
        defaultLocation: DefaultLocation?.ToModel(),
        isCritical: IsCritical,
        alertTimeInventories: AlertTimeInventories?.Select(item => item.ToModel()).ToList()
        );
    }

    public void UpdateDb(InventoryModel inventory, AimasContext aimas)
    {
      Description = inventory.Description;
      CurrentLocation = aimas.GetDbLocation(inventory.CurrentLocation);
      DefaultLocation = inventory.DefaultLocation?.ID == default(long)
        ? null
        : aimas.GetDbLocation(inventory.DefaultLocation);
      IsCritical = inventory.IsCritical;

      if (!string.IsNullOrEmpty(inventory.Name))
        Name = inventory.Name;

      if (inventory.ExpirationDate != default)
        ExpirationDate = inventory.ExpirationDate;

      if (inventory.MaintenanceIntervalDays != default)
        MaintenanceIntervalDays = inventory.MaintenanceIntervalDays;

      // Update Alerts
      var addAlerts = inventory.AlertTimeInventories.Where(item => AlertTimeInventories.Find(item2 => item2.ID == item.ID) == null).Select(item => item.CreateNewDbModel()).ToList();
      var removeAlerts = AlertTimeInventories.Where(item => inventory.AlertTimeInventories.Find(item2 => item2.ID == item.ID) == null).ToList();
      AlertTimeInventories.AddRange(addAlerts);
      removeAlerts.ForEach(item => AlertTimeInventories.Remove(item));
    }
  }
}
