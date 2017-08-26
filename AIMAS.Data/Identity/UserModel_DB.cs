using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AIMAS.Data.Identity
{
  public class UserModel_DB : IdentityUser<Guid>
  {
    [Key]
    public override Guid Id { get => base.Id; set => base.Id = value; }

    [Required, Column(TypeName = "varchar(50)")]
    public override string UserName { get => base.UserName; set => base.UserName = value; }

    [Required, Column(TypeName = "varchar(50)")]
    public string FirstName { get; set; }

    [Required, Column(TypeName = "varchar(50)")]
    public string LastName { get; set; }

    [Required, Column(TypeName = "varchar(100)")]
    public override string Email { get => base.Email; set => base.Email = value; }

    public UserModel_DB() : base()
    {
      Id = new Guid();
    }

    public UserModel_DB(string userName, string firstName, string lastName, string email) : this()
    {
      UserName = userName;
      Email = email;
      FirstName = firstName;
      LastName = lastName;
    }
  }
}
