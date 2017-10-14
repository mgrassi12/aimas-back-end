using System;
using AIMAS.Data.Inventory;
using System.Collections.Generic;
using System.Linq;

namespace AIMAS.Data.Models
{
  public class InventoryModel : IAimasModel<InventoryModel_DB>
  {
    public long ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public long MaintenanceIntervalDays { get; set; }
    public LocationModel CurrentLocation { get; set; }
    public LocationModel DefaultLocation { get; set; }
    public bool IsCritical { get; set; }
    public List<InventoryAlertTimeModel> AlertTimeInventories { get; set; }

    public InventoryModel()
    {

    }

    public InventoryModel(
      string name,
      DateTime expiration,
      long intervalDays,
      LocationModel currentLocation,
      LocationModel defaultLocation = default,
      string description = default,
      bool isCritical = default,
      List<InventoryAlertTimeModel> alertTimeInventories = default,
      long id = default
      )
    {
      ID = id;
      Name = name;
      Description = description;
      ExpirationDate = expiration;
      MaintenanceIntervalDays = intervalDays;
      CurrentLocation = currentLocation;
      DefaultLocation = defaultLocation;
      IsCritical = isCritical;
      AlertTimeInventories = alertTimeInventories ?? new List<InventoryAlertTimeModel>();
    }

    public InventoryModel_DB CreateNewDbModel(AimasContext aimas)
    {
      var dbCurrentLocation = aimas.GetDbLocation(CurrentLocation);
      var dbDefaultLocation = DefaultLocation?.ID == default(long) ? null : aimas.GetDbLocation(DefaultLocation);

      return new InventoryModel_DB(
        id: ID,
        name: Name,
        description: Description,
        expire: ExpirationDate,
        intervalDays: MaintenanceIntervalDays,
        currentLocation: dbCurrentLocation,
        defaultLocation: dbDefaultLocation,
        isCritical: IsCritical,
        alertTimeInventories: AlertTimeInventories?.Select(item => item.CreateNewDbModel()).ToList()
        );
    }
  }

}
