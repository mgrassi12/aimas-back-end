using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("categoryEntry")]
  public class CategoryEntryModel_DB
    {
    [Key]
    public CategoryModel_DB Category { get; set; }

    [Key]
    public InventoryModel_DB Inventory { get; set; }

    public CategoryEntryModel_DB(CategoryModel_DB category, InventoryModel_DB inventory)
    {
      Category = category;
      Inventory = inventory;
    }
  }
}
