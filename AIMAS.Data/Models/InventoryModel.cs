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

    public InventoryModel_DB ToDB()
    {
      return new InventoryModel_DB()
      {
        ID = ID,
        Name = Name,
        Description = Description,
        ExpirationDate = ExpirationDate,
        MaintanceDate = MaintanceDate,
        Location = new LocationModel_DB()
        {
          ID = Location.ID
        }
      };
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
