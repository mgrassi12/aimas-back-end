using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class AlertTimeModel : IAimasModel<AlertTimeModel_DB>
  {
    public InventoryModel Inventory { get; set; }
    public long ID { get; set; }
    public AlertTimeType Type { get; set; }
    public long DaysBefore { get; set; }

    public AlertTimeModel()
    {

    }

    public AlertTimeModel(InventoryModel inventory, AlertTimeType type, long daysBefore, long id = default)
    {
      Inventory = inventory;
      ID = id;
      Type = type;
      DaysBefore = daysBefore;
    }

    public AlertTimeModel_DB ToDbModel()
    {
      return new AlertTimeModel_DB(inventory: Inventory?.ToDbModel(), id: ID, type: Type, daysBefore: DaysBefore);
    }
  }
}
