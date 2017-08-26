using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIMAS.Data.Identity;

namespace AIMAS.Data.Models
{
  public class UserModel
  {
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public UserModel()
    {

    }

    public static UserModel FromDBUser(UserModel_DB userDB)
    {
      return new UserModel()
      {
        UserName = userDB.UserName,
        Email = userDB.Email,
        FirstName = userDB.FirstName,
        LastName = userDB.LastName
      };
    }

    public UserModel_DB ToUserDB()
    {
      return new UserModel_DB()
      {
        UserName = UserName,
        Email = Email,
        FirstName = FirstName,
        LastName = LastName
      };
    }
  }
}
