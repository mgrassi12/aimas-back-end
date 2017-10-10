using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class LocationModel : IAimasModel<LocationModel_DB>
  {
    public long ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public LocationModel()
    {

    }

    public LocationModel(string name, string description = default, long id = default) : this()
    {
      ID = id;
      Name = name;
      Description = description;
    }

    public LocationModel_DB CreateNewDbModel(AimasContext aimas)
    {
      return new LocationModel_DB(id: ID, name: Name, description: Description);
    }
  }
}
