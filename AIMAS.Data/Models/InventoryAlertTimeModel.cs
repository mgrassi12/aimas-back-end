using AIMAS.Data.Inventory;
using System;

namespace AIMAS.Data.Models
{
  public class InventoryAlertTimeModel : IAimasModel<InventoryAlertTimeModel_DB>
  {
    public InventoryModel Inventory { get; set; }
    public long ID { get; set; }
    public AlertTimeType Type { get; set; }
    public long DaysBefore { get; set; }
    public DateTime? SentTime { get; set; }

    public InventoryAlertTimeModel(InventoryModel inventory, AlertTimeType type, long daysBefore, DateTime? sentTime = default, long id = default)
    {
      Inventory = inventory;
      ID = id;
      Type = type;
      DaysBefore = daysBefore;
      SentTime = sentTime;
    }

    public InventoryAlertTimeModel_DB CreateNewDbModel(AimasContext aimas)
    {
      var dbInventory = aimas.GetDbInventory(Inventory);
      return new InventoryAlertTimeModel_DB(inventory: dbInventory, id: ID, type: Type, daysBefore: DaysBefore, sentTime: SentTime);
    }
  }
}
