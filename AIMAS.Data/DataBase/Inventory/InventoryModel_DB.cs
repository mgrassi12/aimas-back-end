using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [Required]
    public LocationModel_DB CurrentLocation { get; set; }

    public LocationModel_DB DefaultLocation { get; set; }

    public bool IsArchived { get; set; }

    public bool IsCritical { get; set; }

    public InventoryModel_DB() : base()
    {

    }

    public InventoryModel_DB(string name, DateTime expire, DateTime maintenance, LocationModel_DB currentLocation, LocationModel_DB defaultLocation, bool isArchived, bool isCritical, string description = null, long id = 0) : this()
    {
      ID = id;
      Name = name;
      Description = description;
      ExpirationDate = expire;
      MaintenanceDate = maintenance;
      CurrentLocation = currentLocation;
      DefaultLocation = DefaultLocation;
      IsArchived = isArchived;
      IsCritical = isCritical;
    }

    public InventoryModel ToModel()
    {
      return new InventoryModel(id: ID, name: Name, description: Description, expiration: ExpirationDate, maintenance: MaintenanceDate, currentLocation: CurrentLocation.ToModel(), defaultLocation: DefaultLocation.ToModel(), isArchived: IsArchived, isCritical: IsCritical);
    }
  }
}
