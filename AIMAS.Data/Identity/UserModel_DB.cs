using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AIMAS.Data.Identity
{
  public class UserModel_DB : IdentityUser<long>
  {
    [Key]
    public override long Id { get => base.Id; set => base.Id = value; }

    [Required, MaxLength(50)]
    public override string UserName { get => base.UserName; set => base.UserName = value; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required, MaxLength(100)]
    public override string Email { get => base.Email; set => base.Email = value; }

    [Required, MaxLength(50)]
    public string Position { get; set; }

    public UserModel_DB() : base()
    {
    }

    public UserModel_DB(string firstName, string lastName, string email, string position, string phone = null) : this()
    {
      UserName = email;
      Email = email;
      FirstName = firstName;
      LastName = lastName;
      Position = position;
      PhoneNumber = phone;
    }
  }
}
