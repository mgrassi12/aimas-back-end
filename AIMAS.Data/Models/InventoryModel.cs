using System;
using System.Collections.Generic;
using System.Text;

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
  }

  public class InventorySearch
  {
    public string ID { get; set; }
    public string Name { get; set; }
    public string Name2 { get; set; }
    public string Name3 { get; set; }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
  }
}
