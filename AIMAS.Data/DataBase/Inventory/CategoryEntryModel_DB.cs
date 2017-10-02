using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("categoryEntry")]
  public class CategoryEntryModel_DB
    {
    [Required]
    public CategoryModel_DB Category { get; set; }

    [Required]
    public InventoryModel_DB Inventory { get; set; }

    [Key]
    public long ID { get; set; }

    public CategoryEntryModel_DB(CategoryModel_DB category, InventoryModel_DB inventory)
    {
      Category = category;
      Inventory = inventory;
      ID = default;
    }
  }
}
