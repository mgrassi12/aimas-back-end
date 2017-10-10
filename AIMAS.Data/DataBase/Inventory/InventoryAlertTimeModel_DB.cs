using AIMAS.Data.Models;
using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations;
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
  public class InventoryAlertTimeModel_DB : IAimasDbModel<InventoryAlertTimeModel>
  {
    [Required]
    public InventoryModel_DB Inventory { get; set; }

    [Key]
    public long ID { get; set; }

    [Required]
    public AlertTimeType Type { get; set; }

    [Required]
    public long DaysBefore { get; set; }

    [Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime? SentTime { get; set; }

    private InventoryAlertTimeModel_DB()
    {
    }

    public InventoryAlertTimeModel_DB(InventoryModel_DB inventory, AlertTimeType type, long daysBefore, DateTime? sentTime = default, long id = default)
    {
      Inventory = inventory;
      ID = id;
      Type = type;
      DaysBefore = daysBefore;
      SentTime = sentTime;
    }

    public InventoryAlertTimeModel ToModel()
    {
      return new InventoryAlertTimeModel(inventory: Inventory.ToModel(), id: ID, type: Type, daysBefore: DaysBefore, sentTime: SentTime);
    }
  }
}
