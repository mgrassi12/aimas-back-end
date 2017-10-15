using AIMAS.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AIMAS.Data.Identity
{
  public class RoleModel_DB : IdentityRole<long>, IAimasDbModel<RoleModel>
  {
    [Key]
    public override long Id { get => base.Id; set => base.Id = value; }

    [Required, MaxLength(50)]
    public override string Name { get => base.Name; set { base.Name = value; base.NormalizedName = value.ToUpper(); } }

    public RoleModel_DB() : base()
    {
    }

    public RoleModel_DB(string name) : this()
    {
      Name = name;
    }

    public RoleModel ToModel()
    {
      return new RoleModel(Name);
    }

    public void UpdateDb(RoleModel model, AimasContext aimas)
    {
      if (!string.IsNullOrEmpty(model.Name))
        Name = model.Name;
    }
  }
}
