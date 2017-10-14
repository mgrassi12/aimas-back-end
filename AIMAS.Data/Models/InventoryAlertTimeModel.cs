using AIMAS.Data.Inventory;
using System;

namespace AIMAS.Data.Models
{
  public class InventoryAlertTimeModel : IAimasModel<InventoryAlertTimeModel_DB>
  {
    public long ID { get; set; }
    public InventoryAlertTimeType Type { get; set; }
    public long DaysBefore { get; set; }
    public DateTime? SentTime { get; set; }

    public InventoryAlertTimeModel(InventoryAlertTimeType type, long daysBefore, DateTime? sentTime = default, long id = default)
    {
      ID = id;
      Type = type;
      DaysBefore = daysBefore;
      SentTime = sentTime;
    }

    public InventoryAlertTimeModel_DB CreateNewDbModel(AimasContext aimas = default)
    {
      return new InventoryAlertTimeModel_DB(id: ID, type: Type, daysBefore: DaysBefore, sentTime: SentTime);
    }
  }
}
