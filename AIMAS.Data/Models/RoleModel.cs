using AIMAS.Data.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIMAS.Data.Models
{
    public class RoleModel: IAimasModel<RoleModel_DB>
    {
    public string Name { get; set; }

    public RoleModel(string name)
    {
      Name = name;
    }

    public RoleModel_DB ToDbModel()
    {
      return new RoleModel_DB(Name);
    }
  }
}
