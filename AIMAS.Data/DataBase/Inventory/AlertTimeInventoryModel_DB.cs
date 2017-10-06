using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AIMAS.Data.Inventory
{
  public enum AlertTimeType
  {
    Inventoy_E_Date = 1,
    Inventory_M_Date = 2,
    ReportAlert_Date = 4,
    Inventory_Date = Inventoy_E_Date | Inventory_M_Date
  }

  [Table("alerttimeinventory")]
  public class AlertTimeInventoryModel_DB
  {

    public long AlertID { get; set; }
    public AlertTimeModel_DB AlertTime { get; set; }

    public long InventoryID { get; set; }
    public InventoryModel_DB Inventory { get; set; }

    public AlertTimeType Type { get; set; }

    private AlertTimeInventoryModel_DB()
    {
    }

    public AlertTimeInventoryModel_DB(long alert, long inventory, AlertTimeType type)
    {
      AlertID = alert;
      InventoryID = inventory;
      Type = type;
    }

    public AlertTimeInventoryModel_DB(AlertTimeModel_DB alert, InventoryModel_DB inventory, AlertTimeType type) : this(alert.ID, inventory.ID, type)
    {
      AlertTime = alert;
      Inventory = inventory;
    }


  }
}
