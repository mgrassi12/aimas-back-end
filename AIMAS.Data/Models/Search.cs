using System;
using System.Collections.Generic;
using System.Text;

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


  public class ReservationSearch : Search
  {
    public string UserName { get; set; }

  }

}
