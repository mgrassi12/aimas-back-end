using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AIMAS.Data.Models;

namespace AIMAS.Data.Inventory
{
  [Table("category")]
  public class CategoryModel_DB : IAimasDbModel<CategoryModel>
  {
    [Key]
    public long ID { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    public List<CategoryInventoryModel_DB> CategoryInventories { get; set; }

    private CategoryModel_DB()
    {
    }

    public CategoryModel_DB(string name, long id = default)
    {
      ID = id;
      Name = name;
      CategoryInventories = new List<CategoryInventoryModel_DB>();
    }

    public CategoryModel ToModel()
    {
      return new CategoryModel(id: ID, name: Name);
    }
  }
}
