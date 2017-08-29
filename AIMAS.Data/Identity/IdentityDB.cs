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
    public AimasContext Aimas { get; set; }
    public UserManager<UserModel_DB> Manager { get; set; }

    public IdentityDB(AimasContext aimas, UserManager<UserModel_DB> userManager)
    {
      Aimas = aimas;
      Manager = userManager;
    }

    public void Initialize()
    {
      // Admin Role
      Aimas.Roles.Add(new RoleModel_DB("Admin"));
      // Admin Role
      Aimas.Roles.Add(new RoleModel_DB("InventoryManager"));
      // User Role
      Aimas.Roles.Add(new RoleModel_DB("User"));
      // Save Changes
      Aimas.SaveChanges();
      // Admin User
      var adminUser = new UserModel_DB("Admin", "Admin", "admin@example.com", "Admin");
      // Add Admin User with Role Admin
      var result = CreateUserAsync(adminUser, "Admin@1").Result;
      if (result.Success)
      {
        AddUserRoleAsync(adminUser, "Admin").Wait();
        AddUserRoleAsync(adminUser, "InventoryManager").Wait();
        AddUserRoleAsync(adminUser, "User").Wait();
      }
      else
      {
        throw new Exception("Failed to Create Admin User");
      }
    }

    public async Task<Result> CreateUserAsync(UserModel_DB user, string password)
    {
      Result result = new Result();
      try
      {
        // Add User
        var createUser = await Manager.CreateAsync(user, password);
        if (createUser.Succeeded)
        {
          // Success
          Aimas.SaveChanges();
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
        var role = await Aimas.Roles.FirstAsync(r => r.Name == roleName);
        await Aimas.UserRoles.AddAsync(NewIdentityRole(user.Id, role.Id));
        Aimas.SaveChanges();
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
      var query = from user in Aimas.Users
                  select new UserModel()
                  {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.FirstName
                  };
      return await query.ToListAsync();
    }
    public async Task<List<UserModel_DB>> GetUsersDBAsync()
    {
      var query = from user in Aimas.Users
                  select user;
      return await query.ToListAsync();
    }

    public async Task<UserModel> GetUserAsync(string email)
    {
      var query = from user in Aimas.Users
                  where user.Email == email
                  select new UserModel()
                  {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.FirstName
                  };
      return await query.FirstAsync();
    }
    public async Task<UserModel_DB> GetUserDBAsync(string email)
    {
      var query = from user in Aimas.Users
                  where user.Email == email
                  select user;
      return await query.FirstAsync();
    }

    #region UTIL

    public IdentityUserRole<long> NewIdentityRole(long usrrID, long roleID)
    {
      return new IdentityUserRole<long>() { UserId = usrrID, RoleId = roleID };
    }

    #endregion
  }
}
