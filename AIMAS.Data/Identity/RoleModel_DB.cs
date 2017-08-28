using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AIMAS.Data.Identity
{
  public class RoleModel_DB : IdentityRole<long>
  {
    [Key]
    public override long Id { get => base.Id; set => base.Id = value; }

    [Required, MaxLength(50)]
    public override string Name { get => base.Name; set => base.Name = value; }

    public RoleModel_DB() : base()
    {
    }

    public RoleModel_DB(string name) : this()
    {
      Name = name;
    }
  }
}
