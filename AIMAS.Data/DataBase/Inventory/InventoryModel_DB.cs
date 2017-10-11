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
  public class InventoryModel_DB : IAimasDbModel<InventoryModel>
  {
    [Key]
    public long ID { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime ExpirationDate { get; set; }

    public long MaintenanceIntervalDays { get; set; }

    [Required]
    public LocationModel_DB CurrentLocation { get; set; }

    public LocationModel_DB DefaultLocation { get; set; }

    public List<InventoryAlertTimeModel_DB> AlertTimeInventories { get; set; }

    public List<CategoryInventoryModel_DB> CategoryInventories { get; set; }

    public List<ReservationInventoryModel_DB> ReservationInventories { get; set; }

    [Required]
    public bool IsCritical { get; set; }

    [Required]
    public bool IsArchived { get; set; }

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
      bool isArchived = default,
      bool isCritical = default,
      List<InventoryAlertTimeModel_DB> alertTimeInventories = default,
      long id = default
      )
    {
      ID = id;
      Name = name;
      Description = description;
      ExpirationDate = expire;
      MaintenanceIntervalDays = intervalDays;
      CurrentLocation = currentLocation;
      DefaultLocation = defaultLocation;
      IsArchived = isArchived;
      IsCritical = isCritical;
      AlertTimeInventories = alertTimeInventories ?? new List<InventoryAlertTimeModel_DB>();
      CategoryInventories = new List<CategoryInventoryModel_DB>();
      ReservationInventories = new List<ReservationInventoryModel_DB>();
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
        isArchived: IsArchived,
        isCritical: IsCritical,
        alertTimeInventories: AlertTimeInventories?.Select(item => item.ToModel()).ToList()
        );
    }

    public void UpdateDb(InventoryModel inventory, AimasContext aimas)
    {
      Description = inventory.Description;      
      IsArchived = inventory.IsArchived;
      IsCritical = inventory.IsCritical;

      if (!string.IsNullOrEmpty(inventory.Name))
        Name = inventory.Name;

      if (inventory.ExpirationDate != default)
        ExpirationDate = inventory.ExpirationDate;

      if (inventory.MaintenanceIntervalDays != default)
        MaintenanceIntervalDays = inventory.MaintenanceIntervalDays;

      if (inventory.CurrentLocation?.ID != -1)
        CurrentLocation = aimas.GetDbLocation(inventory.CurrentLocation);

      if (inventory.DefaultLocation?.ID != -1)
        DefaultLocation = aimas.GetDbLocation(inventory.DefaultLocation);

    }
  }
}
