using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AIMAS.Data.Identity;
using AIMAS.Data.Models;


namespace AIMAS.API.Controllers
{
  [Route("api/auth")]
  public class AuthController : Controller
  {
    private ILogger log;

    private IdentityDB IdentityDB { get; }
    private SignInManager<UserModel_DB> SignInManager { get; }

    public AuthController(IdentityDB identityController, SignInManager<UserModel_DB> signInManager)
    {
      log = Startup.LoggerFactory.CreateLogger<AuthController>();
      IdentityDB = identityController;
      SignInManager = signInManager;
    }

    [HttpPost]
    [Route("register/user")]
    [Authorize(Roles = "Admin")]
    public async Task<Result> RegisterUser([FromBody] UserModel user, [FromBody]string password)
    {

      var userDB = user.ToUserDB();

      var result = await IdentityDB.CreateUserWithRoleAsync(userDB, "User", password);

      if (result.Success)
      {
        // Email Confirmm (https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?tabs=aspnetcore2x%2Csql-server)

      }

      return result;
    }

    [HttpGet]
    [Route("users")]
    //[Authorize(Roles = "Admin")]
    public Result<List<UserModel>> GetUsers()
    {
      var result = new Result<List<UserModel>>();

      try
      {
        var users = IdentityDB.GetUsers();
        result.Success = true;
        result.ReturnObj = users;

      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }


  }
}
