using AIMAS.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  public enum AlertTimeType
  {
    Inventoy_E_Date,
    Inventory_M_Date
  }

  public enum AlertTimeLinkType
  {
    Inventory,
    Reservation
  }

  [Table("alertTime")]
  public class AlertTimeModel_DB : IAimasDbModel<AlertTimeModel>
  {

    [Key]
    public long ID { get; set; }

    [Required]
    public AlertTimeType Type { get; set; }

    [Required]
    public long DaysBefore { get; set; }

    //[Required]
    //public AlertTimeLinkType LinkType;


    [Required]
    public InventoryModel_DB Inventory { get; set; }

    public AlertTimeModel_DB()
    {

    }

    public AlertTimeModel_DB(InventoryModel_DB inventory, AlertTimeType type, long daysBefore, long id = default)
    {
      Inventory = inventory;
      ID = id;
      Type = type;
      DaysBefore = daysBefore;
    }

    public AlertTimeModel ToModel()
    {
      return new AlertTimeModel(inventory: Inventory.ToModel(), id: ID, type: Type, daysBefore: DaysBefore);
    }

    public void UpdateDb(AlertTimeModel alertTime, AimasContext aimas)
    {
      Type = alertTime.Type;

      if (alertTime.DaysBefore != default)
        DaysBefore = alertTime.DaysBefore;

      var newInventory = aimas.GetDbInventory(alertTime.Inventory);
      if (newInventory != null)
        Inventory = newInventory;
    }
  }
}
