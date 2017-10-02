using System;
using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class InventoryModel : IAimasModel<InventoryModel_DB>
  {
    public long ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public long MaintenanceIntervalNumber { get; set; }
    public string MaintenanceIntervalType { get; set; }
    public LocationModel CurrentLocation { get; set; }
    public LocationModel DefaultLocation { get; set; }
    public bool IsArchived { get; set; }
    public bool IsCritical { get; set; }

    public InventoryModel()
    {

    }

    public InventoryModel(string name, DateTime expiration, DateTime maintenanceDate, LocationModel currentLocation, LocationModel defaultLocation, string description = default, long intervalNumber = default, string intervalType = default, bool isArchived = default, bool isCritical = default, long id = default) : this()
    {
      ID = id;
      Name = name;
      Description = description;
      ExpirationDate = expiration;
      MaintenanceDate = maintenanceDate;
      MaintenanceIntervalNumber = intervalNumber;
      MaintenanceIntervalType = intervalType;
      CurrentLocation = currentLocation;
      DefaultLocation = defaultLocation;
      IsArchived = isArchived;
      IsCritical = isCritical;
    }

    public InventoryModel_DB ToDbModel()
    {
      return new InventoryModel_DB(id: ID, name: Name, description: Description, expire: ExpirationDate, maintenanceDate: MaintenanceDate, intervalNumber: MaintenanceIntervalNumber, intervalType: MaintenanceIntervalType, currentLocation: CurrentLocation.ToDbModel(), defaultLocation: DefaultLocation.ToDbModel(), isArchived : IsArchived, isCritical: IsCritical);
    }
  }

  public class InventorySearch
  {
    public long? ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public LocationModel Location { get; set; }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public InventorySearch()
    {
      PageIndex = 0;
      PageSize = 25;
    }
  }
}
