using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIMAS.Data.Identity;

namespace AIMAS.Data.Models
{
  public class UserModel
  {
    public long Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }

    public UserModel()
    {

    }

    public UserModel(string email, string firstName, string lastName, string position, long id = default) : this()
    {
      Id = id;
      FirstName = firstName;
      LastName = lastName;
      Position = position;
    }

    public UserModel_DB ToDBModel()
    {
      return new UserModel_DB(id: Id, email: Email, firstName: FirstName, lastName: LastName, position: Position);
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
