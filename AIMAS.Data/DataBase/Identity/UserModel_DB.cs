using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using AIMAS.Data.Models;
using System;

namespace AIMAS.Data.Identity
{
  public class UserModel_DB : IdentityUser<long>, IAimasDbModelWithUpdate<UserModel>
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

    public UserModel_DB(string firstName, string lastName, string email, string position, string phone = default, long id = default) : this()
    {
      Id = id;
      UserName = email;
      Email = email;
      FirstName = firstName;
      LastName = lastName;
      Position = position;
      PhoneNumber = phone;
    }

    public UserModel ToModel()
    {
      return new UserModel(id: Id, email:Email, firstName: FirstName, lastName: LastName, position: Position);
    }

    public void UpdateDb(UserModel user, AimasContext aimas)
    {
      var setFunctions = new Action<string>[] {s => { UserName = s; }, s => { FirstName = s; }, s => { LastName = s; }, s => { Email = s; }, s => { Position = s; }};
      var newProperties = new string[] { user.Email, user.FirstName, user.LastName, user.Email, user.Position };
      for (int i = 0; i < setFunctions.Length; i++)
      {
        if (!string.IsNullOrEmpty(newProperties[i]))
          setFunctions[i](newProperties[i]);
      }
    }
  }
}
