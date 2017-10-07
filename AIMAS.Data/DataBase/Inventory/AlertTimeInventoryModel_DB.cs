using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  public enum AlertTimeType
  {
    Inventory_E_Date = 1,
    Inventory_M_Date = 2,
    ReportAlert_Date = 4,
    Inventory_Date = Inventory_E_Date | Inventory_M_Date
  }

  [Table("alerttimeinventory")]
  public class AlertTimeInventoryModel_DB
  {

    public long AlertID { get; set; }
    public AlertTimeModel_DB AlertTime { get; set; }

    public long InventoryID { get; set; }
    public InventoryModel_DB Inventory { get; set; }

    public AlertTimeType Type { get; set; }

    [Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime? SentTime { get; set; }

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
