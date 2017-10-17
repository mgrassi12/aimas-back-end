using AIMAS.Data.Identity;
using System.Collections.Generic;

namespace AIMAS.Data.Models
{
  public class UserModel : IAimasModel<UserModel_DB>
  {
    public long Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public List<RoleModel> UserRoles { get; set; }

    public UserModel()
    {

    }

    public UserModel(long id) : this()
    {
      Id = id;
    }

    public UserModel(string email, string firstName, string lastName, string position, long id = default, List<RoleModel> roles = default) : this()
    {
      Id = id;
      Email = email;
      FirstName = firstName;
      LastName = lastName;
      Position = position;
      UserRoles = roles ?? new List<RoleModel>();
    }
    
    public UserModel_DB CreateNewDbModel(AimasContext aimas = null)
    {
      return new UserModel_DB(id: Id, email: Email, firstName: FirstName, lastName: LastName, position: Position);
    }

    public UserModel SetRoles(List<RoleModel> roles)
    {
      UserRoles = roles;
      return this;
    }

  }

  public class RegisterModel : UserModel
  {
    public string Password { get; set; }
  }

  public class UserLoginModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }
}
