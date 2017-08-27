using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AIMAS.Data.Identity;
using AIMAS.Data.Models;


namespace AIMAS.Data.Identity
{
  public class IdentityDB
  {
    public IdentityContext Identity { get; set; }
    public UserManager<UserModel_DB> Manager { get; set; }

    public IdentityDB(IdentityContext identityContext, UserManager<UserModel_DB> userManager)
    {
      Identity = identityContext;
      Manager = userManager;
    }

    public void Initialize()
    {
      //context.Database.Migrate();

      if (!Identity.Roles.AnyAsync().Result)
      {
        // Admin Role
        Identity.Roles.Add(new RoleModel_DB("Admin"));
        // Admin Role
        Identity.Roles.Add(new RoleModel_DB("InventoryManager"));
        // User Role
        Identity.Roles.Add(new RoleModel_DB("User"));
        // Save Changes
        Identity.SaveChanges();
      }

      if (!Identity.Users.AnyAsync().Result)
      {
        // Admin User
        var adminUser = new UserModel_DB("Admin", "Admin", "Admin", "admin@example.com");
        // Add Admin User with Role Admin
        CreateUserWithRoleAsync(adminUser, "Admin", "Admin@1").Wait();
        AddUserRoleAsync(adminUser, "InventoryManager").Wait();
        AddUserRoleAsync(adminUser, "User").Wait();

        // Save Changes
      }
    }

    public async Task<Result> CreateUserWithRoleAsync(UserModel_DB user, string roleName, string password)
    {
      Result result = new Result();
      try
      {
        // Add User
        var createUser = await Manager.CreateAsync(user, password);
        if (createUser.Succeeded)
        {
          // Add UserRole
          var role = await Identity.Roles.FirstAsync(r => r.Name == roleName);
          var roleResult = await AddUserRoleAsync(user, roleName);
          result.MergeResult(result);

          // Success
          result.Success = true;
        }
        else
        {
          // Failed to Create User
          result.ErrorMessage = "Something went wrong while Create User";
          result.AddIdentityErrors(createUser.Errors);
        }
      }
      catch (Exception ex)
      {
        result.ErrorMessage = "Something went wrong while Setting up the new User";
        result.AddException(ex);
      }

      // Return Result;
      return result;
    }

    public async Task<Result> AddUserRoleAsync(UserModel_DB user, string roleName)
    {
      var result = new Result();
      try
      {
        // Add UserRole
        var role = await Identity.Roles.FirstAsync(r => r.Name == roleName);
        await Identity.UserRoles.AddAsync(NewIdentityRole(user.Id, role.Id));
        Identity.SaveChanges();
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }
      return result;
    }

    public async Task<List<UserModel>> GetUsersAsync()
    {
      var list = from user in Identity.Users
                 select new UserModel()
                 {
                   UserName = user.UserName,
                   Email = user.Email,
                   FirstName = user.FirstName,
                   LastName = user.FirstName
                 };
      return await list.ToListAsync();
    }
    public async Task<List<UserModel_DB>> GetUsersDBAsync()
    {
      var list = from user in Identity.Users
                 select user;
      return await list.ToListAsync();
    }

    public async Task<UserModel> GetUserAsync(string email)
    {
      var list = from user in Identity.Users
                 where user.Email == email
                 select new UserModel()
                 {
                   UserName = user.UserName,
                   Email = user.Email,
                   FirstName = user.FirstName,
                   LastName = user.FirstName
                 };
      return await list.FirstAsync();
    }
    public async Task<UserModel_DB> GetUserDBAsync(string email)
    {
      var list = from user in Identity.Users
                 where user.Email == email
                 select user;
      return await list.FirstAsync();
    }

    #region UTIL

    public IdentityUserRole<Guid> NewIdentityRole(Guid usrrID, Guid roleID)
    {
      return new IdentityUserRole<Guid>() { UserId = usrrID, RoleId = roleID };
    }

    #endregion
  }
}
