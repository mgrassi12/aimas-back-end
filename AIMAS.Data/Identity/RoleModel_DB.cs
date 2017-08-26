using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AIMAS.Data.Identity
{
  public class RoleModel_DB : IdentityRole<Guid>
  {
    [Key]
    public override Guid Id { get => base.Id; set => base.Id = value; }

    [Required, Column(TypeName = "varchar(50)")]
    public override string Name { get => base.Name; set => base.Name = value; }

    public RoleModel_DB() : base()
    {
      Id = new Guid();
    }

    public RoleModel_DB(string name) : this()
    {
      Name = name;
    }
  }
}
