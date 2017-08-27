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
using AIMAS.API.Models;


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

    [HttpGet]
    [Route("info")]
    [AllowAnonymous]
    public Result<CurrentUserInfo> GetCurrentUserInfo()
    {
      var result = new Result<CurrentUserInfo>();
      try
      {
        result.ReturnObj = new CurrentUserInfo()
        {
          IsAuth = User.Identity.IsAuthenticated,
          IsAdmin = User.IsInRole("Admin")
        };
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.ErrorMessage = "Something went wrong while geting the current user information";
        result.AddException(ex);
      }
      return result;
    }

    [HttpPost]
    [Route("register/user")]
    [Authorize(Roles = "Admin")]
    public async Task<Result> RegisterUser([FromBody] UserPasswordModel userDetails)
    {
      var userDB = userDetails.ToUserDB();
      var result = await IdentityDB.CreateUserWithRoleAsync(userDB, "User", userDetails.Password);
      if (result.Success)
      {
        // Email Confirmm (https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?tabs=aspnetcore2x%2Csql-server)

      }
      return result;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<Result> Login([FromBody]UserPasswordModel userDetails)
    {
      var result = new Result();
      try
      {
        var user = await IdentityDB.Manager.FindByEmailAsync(userDetails.Email);
        var signinResult = await SignInManager.PasswordSignInAsync(user, userDetails.Password, false, false);
        if (signinResult.Succeeded)
        {
          log.LogInformation("{0} loged in", userDetails.Email);
          result.Success = true;
        }
        else
        {
          // Failed to Signin
          throw new Exception("Something went wrong");
        }

      }
      catch (Exception ex)
      {
        result.ErrorMessage = "Something went wrong while Loging In";
        result.AddException(ex);
      }
      return result;
    }

    [HttpGet]
    [Route("logout")]
    [Authorize]
    public async Task<Result> LogOut()
    {
      var result = new Result();
      try
      {
        var email = IdentityDB.Manager.GetUserAsync(User).Result.Email;
        await SignInManager.SignOutAsync();
        log.LogInformation("{0} Loged Out", email);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.ErrorMessage = "Something went wrong while Loging Out";
        result.AddException(ex);
      }
      return result;
    }

    [HttpGet]
    [Route("users")]
    [Authorize(Roles = "Admin")]
    public async Task<Result<List<UserModel>>> GetUsers()
    {
      var result = new Result<List<UserModel>>();
      try
      {
        var users = await IdentityDB.GetUsersAsync();
        result.ReturnObj = users;
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.ErrorMessage = "Something went wrong while getting Users";
        result.AddException(ex);
      }
      return result;
    }

  }
}
