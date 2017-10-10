using AIMAS.Data.Identity;

namespace AIMAS.Data.Models
{
  public class RoleModel: IAimasModel<RoleModel_DB>
    {
    public string Name { get; set; }

    public RoleModel(string name)
    {
      Name = name;
    }

    public RoleModel_DB CreateNewDbModel(AimasContext aimas)
    {
      return new RoleModel_DB(Name);
    }
  }
}
