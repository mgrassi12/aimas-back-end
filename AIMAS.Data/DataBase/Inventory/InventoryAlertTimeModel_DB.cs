using AIMAS.Data.Models;
using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  public enum InventoryAlertTimeType
  {
    Inventory_E_Date = 1,
    Inventory_M_Date = 2
  }

  [Table("alerttimeinventory")]
  public class InventoryAlertTimeModel_DB : IAimasDbModel<InventoryAlertTimeModel>
  {
    [Required]
    public InventoryModel_DB Inventory { get; set; }

    [Key]
    public long ID { get; set; }

    [Required]
    public InventoryAlertTimeType Type { get; set; }

    [Required]
    public long DaysBefore { get; set; }

    [Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime? SentTime { get; set; }

    private InventoryAlertTimeModel_DB()
    {
    }

    public InventoryAlertTimeModel_DB(InventoryAlertTimeType type, long daysBefore, InventoryModel_DB inventory = default, DateTime? sentTime = default, long id = default)
    {
      Inventory = inventory;
      ID = id;
      Type = type;
      DaysBefore = daysBefore;
      SentTime = sentTime;
    }

    public InventoryAlertTimeModel ToModel()
    {
      return new InventoryAlertTimeModel(id: ID, type: Type, daysBefore: DaysBefore, sentTime: SentTime);
    }
  }
}
