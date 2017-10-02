using AIMAS.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("alertTime")]
  public class AlertTimeModel_DB : IAimasDbModel<AlertTimeModel>
    {
    [Required]
    public InventoryModel_DB Inventory { get; set; }

    [Key]
    public long ID { get; set; }

    [Required]
    public string Type { get; set; }

    [Required]
    public long DaysBefore { get; set; }

    public AlertTimeModel_DB(InventoryModel_DB inventory, string type, long daysBefore, long id = default)
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
  }
}
