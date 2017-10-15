using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using AIMAS.Data.Models;

namespace AIMAS.Data.Identity
{
  public class UserModel_DB : IdentityUser<long>, IAimasDbModel<UserModel>
  {
    [Key]
    public override long Id { get => base.Id; set => base.Id = value; }

    [Required, MaxLength(50)]
    public override string UserName { get => base.UserName; set { base.UserName = value; base.NormalizedUserName = value.ToUpper(); } }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required, MaxLength(100)]
    public override string Email { get => base.Email; set { base.Email = value; base.NormalizedEmail = value.ToUpper(); } }

    [Required, MaxLength(50)]
    public string Position { get; set; }

    //public List<IdentityUserRole<long>> Roles { get; set; }

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
      return new UserModel(id: Id, email: Email, firstName: FirstName, lastName: LastName, position: Position);
    }

    public void UpdateDb(UserModel user)
    {
      FirstName = user.FirstName ?? FirstName;
      LastName = user.LastName ?? LastName;
      Position = user.Position ?? Position;

      if (!string.IsNullOrEmpty(user.Email))
      {
        Email = user.Email;
        UserName = user.Email;
      }
    }
  }
}
