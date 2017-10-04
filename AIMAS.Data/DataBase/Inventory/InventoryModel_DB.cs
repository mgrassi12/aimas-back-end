using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
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

    // To be removed
    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime MaintenanceDate { get; set; }
    //

    public long MaintenanceIntervalDays { get; set; }

    [Required]
    public LocationModel_DB CurrentLocation { get; set; }

    public LocationModel_DB DefaultLocation { get; set; }

    public List<AlertTimeModel_DB> AlertTimes { get; set; }

    public List<CategoryInventoryModel_DB> CategoryInventories { get; set; }

    public List<ReservationInventoryModel_DB> ReservationInventories { get; set; }

    [Required]
    public bool IsCritical { get; set; }

    [Required]
    public bool IsArchived { get; set; }

    public InventoryModel_DB()
    {

    }

    public InventoryModel_DB(
      string name,
      DateTime expire,
      DateTime maintenanceDate,
      LocationModel_DB currentLocation,
      LocationModel_DB defaultLocation,
      string description = default,
      long intervalDays = 0,
      bool isArchived = default,
      bool isCritical = default,
      long id = default)
    {
      ID = id;
      Name = name;
      Description = description;
      ExpirationDate = expire;
      MaintenanceDate = maintenanceDate;
      MaintenanceIntervalDays = intervalDays;
      CurrentLocation = currentLocation;
      DefaultLocation = defaultLocation;
      AlertTimes = new List<AlertTimeModel_DB>();
      CategoryInventories = new List<CategoryInventoryModel_DB>();
      ReservationInventories = new List<ReservationInventoryModel_DB>();
      IsArchived = isArchived;
      IsCritical = isCritical;
    }

    public InventoryModel ToModel()
    {
      return new InventoryModel(
        id: ID,
        name: Name,
        description: Description,
        expiration: ExpirationDate,
        maintenanceDate: MaintenanceDate,
        intervalDays: MaintenanceIntervalDays,
        currentLocation: CurrentLocation?.ToModel(),
        defaultLocation: DefaultLocation?.ToModel(),
        isArchived: IsArchived,
        isCritical: IsCritical);
    }
  }
}
