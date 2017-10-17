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

    public AuthController(IdentityDB identiryDB, SignInManager<UserModel_DB> signInManager)
    {
      log = Startup.LoggerFactory.CreateLogger<AuthController>();
      IdentityDB = identiryDB;
      SignInManager = signInManager;
    }

    [HttpGet]
    [Route("info")]
    [AllowAnonymous]
    public ResultObj<CurrentUserInfo> GetCurrentUserInfo()
    {
      var result = new ResultObj<CurrentUserInfo>();
      try
      {
        result.ReturnObj = new CurrentUserInfo
        {
          IsAuth = User.Identity.IsAuthenticated
        };
        var user = IdentityDB.Manager.GetUserAsync(User).Result;
        if (user != null)
        {
          result.ReturnObj.User = user.ToModel();
          result.ReturnObj.User.UserRoles = IdentityDB.Manager.GetRolesAsync(user).Result.Select(x => new RoleModel(x)).ToList();
        }
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
    [Authorize(Roles = Roles.Admin)]
    public async Task<Result> RegisterUser([FromBody] RegisterModel registerModel)
    {
      var userModel = registerModel.CreateNewDbModel();
      var result = await IdentityDB.CreateUserAsync(userModel, registerModel.Password);
      if (result.Success)
      {
        // Email Confirmm (https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?tabs=aspnetcore2x%2Csql-server)
        if (registerModel.UserRoles != null)
          registerModel.UserRoles.ForEach(role => result.MergeResult(IdentityDB.AddUserRoleAsync(userModel, role.Name).Result));
      }
      return result;
    }

    [HttpPost]
    [Route("users/update")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Result> UpdateUser([FromBody]UserModel user)
    {
      var result = new Result();

      try
      {
        await IdentityDB.UpdateUser(user);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }

    [HttpGet]
    [Route("users/remove/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public Result RemoveUser(long id)
    {
      var result = new Result();

      try
      {
        IdentityDB.RemoveUser(id);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<Result> Login([FromBody]UserLoginModel loginModel)
    {
      var result = new Result();
      try
      {
        var user = await IdentityDB.Manager.FindByEmailAsync(loginModel.Email);
        var signinResult = await SignInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
        if (signinResult.Succeeded)
        {
          log.LogInformation($"{loginModel.Email} logged in");
          result.Success = true;
        }
        else
        {
          // Failed to Signin
          throw new Exception("Login Failed");
        }

      }
      catch (Exception ex)
      {
        result.ErrorMessage = "Email or Password is incorect";
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
        log.LogInformation("{0} Logged Out", email);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.ErrorMessage = "Something went wrong while Logging Out";
        result.AddException(ex);
      }
      return result;
    }

    [HttpGet]
    [Route("users")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ResultObj<List<UserModel>>> GetUsers()
    {
      var result = new ResultObj<List<UserModel>>();
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

    [HttpPost]
    [Route("user/search")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<PageResultObj<List<UserModel>>> SearchUsers([FromBody]UserSearch search)
    {
      var result = new PageResultObj<List<UserModel>>();

      try
      {
        var items = await IdentityDB.GetUsersAsync(search);
        result.Success = true;
        result.ReturnObj = items.list;
        result.TotalCount = items.TotalCount;
        result.PageIndex = search.PageIndex;
        result.PageSize = search.PageSize;

      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }

    [HttpGet]
    [Route("roles")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ResultObj<List<RoleModel>>> GetRoles()
    {
      var result = new ResultObj<List<RoleModel>>();
      try
      {
        var roles = await IdentityDB.GetRolesAsync();
        result.ReturnObj = roles;
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
