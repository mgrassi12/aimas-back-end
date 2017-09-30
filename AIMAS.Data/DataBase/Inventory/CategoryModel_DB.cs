using AIMAS.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("category")]
  public class CategoryModel_DB : IAimasDbModel<CategoryModel>
  {
    [Key]
    public long ID { get; set; }
    
    [Required, MaxLength(255)]
    public string Name { get; set; }

    public CategoryModel_DB(string name, long id = default)
    {
      ID = id;
      Name = name;
    }

    public CategoryModel ToModel()
    {
      return new CategoryModel(id: ID, name: Name);
    }
  }
}
