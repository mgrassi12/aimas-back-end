using System;
using System.Collections.Generic;
using System.Text;
using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class InventoryModel
  {
    public long ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime MaintanceDate { get; set; }
    public LocationModel Location { get; set; }

    public InventoryModel()
    {

    }

    public InventoryModel(string name, DateTime expiration, DateTime maintance, LocationModel location, string description = default, long id = default) : this()
    {
      ID = id;
      Name = name;
      Description = description;
      ExpirationDate = expiration;
      MaintanceDate = maintance;
      Location = Location;
    }

    public InventoryModel_DB ToDBModel()
    {
      return new InventoryModel_DB(id: ID, name: Name, description: Description, expire: ExpirationDate, maintance: MaintanceDate, location: Location.ToDBModel());
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
