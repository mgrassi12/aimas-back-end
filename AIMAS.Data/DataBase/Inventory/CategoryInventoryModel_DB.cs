using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("categoryIdentity")]
  public class CategoryInventoryModel_DB
  {
    public long CategoryID { get; set; }
    public CategoryModel_DB Category { get; set; }

    public long InventoryID { get; set; }
    public InventoryModel_DB Inventory { get; set; }

    public CategoryInventoryModel_DB(long category, long inventory)
    {
      CategoryID = category;
      InventoryID = inventory;
    }

    public CategoryInventoryModel_DB(CategoryModel_DB category, InventoryModel_DB inventory) : this(category.ID, inventory.ID)
    {
      Category = category;
      Inventory = inventory;
    }
  }
}
