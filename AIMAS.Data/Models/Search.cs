using System;
using System.Collections.Generic;
using System.Text;
using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class Search
  {
    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public Search()
    {
      PageIndex = 0;
      PageSize = 25;
    }
  }



  public class InventorySearch : Search
  {
    public long? ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

  }

  public class ReportSearch : Search
  {
    public long? InventoryId { get; set; }
    public ReportType? Type { get; set; }
  }

  public class ReservationSearch : Search
  {
    public string UserName { get; set; }

  }

  public class UserSearch : Search
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

  }

}
