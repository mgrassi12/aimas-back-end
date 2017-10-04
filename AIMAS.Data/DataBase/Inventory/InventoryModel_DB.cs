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

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime MaintenanceDate { get; set; }

    public long MaintenanceIntervalNumber { get; set; }

    public string MaintenanceIntervalType { get; set; }

    [Required]
    public LocationModel_DB CurrentLocation { get; set; }

    public LocationModel_DB DefaultLocation { get; set; }

    public List<CategoryInventoryModel_DB> CategoryInventory { get; set; }

    public List<ReservationInventoryModel_DB> ReservationInventory { get; set; }

    [Required]
    public bool IsCritical { get; set; }

    [Required]
    public bool IsArchived { get; set; }

    private InventoryModel_DB()
    {
    }

    public InventoryModel_DB(string name, DateTime expire, DateTime maintenanceDate, LocationModel_DB currentLocation, LocationModel_DB defaultLocation, string description = null, long intervalNumber = 0, string intervalType = null, bool isArchived = default, bool isCritical = default, long id = default)
    {
      ID = id;
      Name = name;
      Description = description;
      ExpirationDate = expire;
      MaintenanceDate = maintenanceDate;
      MaintenanceIntervalNumber = intervalNumber;
      MaintenanceIntervalType = intervalType;
      CurrentLocation = currentLocation;
      DefaultLocation = defaultLocation;
      IsArchived = isArchived;
      IsCritical = isCritical;
    }

    public InventoryModel ToModel()
    {
      return new InventoryModel(id: ID, name: Name, description: Description, expiration: ExpirationDate, maintenanceDate: MaintenanceDate, intervalNumber: MaintenanceIntervalNumber, intervalType: MaintenanceIntervalType, currentLocation: CurrentLocation.ToModel(), defaultLocation: DefaultLocation.ToModel(), isArchived: IsArchived, isCritical: IsCritical);
    }
  }
}
