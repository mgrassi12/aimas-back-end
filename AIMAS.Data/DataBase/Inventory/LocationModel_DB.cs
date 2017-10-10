using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AIMAS.Data.Models;

namespace AIMAS.Data.Inventory
{
  [Table("location")]
  public class LocationModel_DB : IAimasDbModelWithUpdate<LocationModel>
  {
    [Key]
    public long ID { get; set; }

    [Required]
    public string Name { get; set; }
    
    public string Description { get; set; }

    private LocationModel_DB()
    {
    }

    public LocationModel_DB(string name, string description = default, long id = default)
    {
      ID = id;
      Name = name;
      Description = description;
    }

    public LocationModel ToModel()
    {
      return new LocationModel(id: ID, name: Name, description: Description);
    }

    public void UpdateDb(LocationModel model, AimasContext aimas)
    {
      Description = model.Description;
      if (!string.IsNullOrEmpty(model.Name))
        Name = model.Name;
    }
  }
}
