using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class CategoryModel : IAimasModel<CategoryModel_DB>
  {
    public long ID { get; set; }
    
    public string Name { get; set; }

    public CategoryModel(string name, long id = default)
    {
      ID = id;
      Name = name;
    }

    public CategoryModel_DB CreateNewDbModel(AimasContext aimas)
    {
      return new CategoryModel_DB(id: ID, name: Name);
    }
  }
}
