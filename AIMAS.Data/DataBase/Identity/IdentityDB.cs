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
    public static readonly List<string> Roles = new List<string>() { "Admin", "InventoryManager", "User" };

    public AimasContext Aimas { get; set; }
    public UserManager<UserModel_DB> Manager { get; set; }

    public IdentityDB(AimasContext aimas, UserManager<UserModel_DB> userManager)
    {
      Aimas = aimas;
      Manager = userManager;
    }

    public void Initialize()
    {
      // Roles
      Roles.ForEach(item => Aimas.Roles.Add(new RoleModel_DB(item)));
      // Save Changes
      Aimas.SaveChanges();
      // Admin User
      var adminUser = new UserModel_DB("Admin", "Admin", "admin@example.com", "Admin");
      var result = CreateUserAsync(adminUser, "Admin@1").Result;
      if (result.Success)
      {
        AddUserRoleAsync(adminUser, Roles[0]).Wait();
      }
      else
      {
        throw new Exception($"Failed to Create Admin User {result.ErrorMessage}");
      }
      var testUser = new UserModel_DB("Test", "User", "aimasbackend@gmail.com", "Test");
      var result2 = CreateUserAsync(testUser, "Testuser@1").Result;
      if (result2.Success)
      {
        AddUserRoleAsync(testUser, Roles[2]).Wait();
      }
      else
      {
        throw new Exception($"Failed to Create Test User {result2.ErrorMessage}");
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
          result.AddIdentityErrors(createUser.Errors);
          throw new Exception("Create User Error");
        }
      }
      catch (Exception ex)
      {
        result.ErrorMessage = $"Something went wrong while Creating new User, {user.Email}";
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
      var users = await GetUsersDBAsync();
      return users.Select(user => user.ToModel()).ToList();
    }
    public async Task<List<UserModel_DB>> GetUsersDBAsync()
    {
      var query = from user in Aimas.Users
                  select user;
      return await query.ToListAsync();
    }

    public async Task<UserModel> GetUserAsync(string email)
    {
      var user = await GetUserDBAsync(email);
      return user.ToModel();
    }
    public async Task<UserModel_DB> GetUserDBAsync(string email)
    {
      var query = from user in Aimas.Users
                  where user.Email == email
                  select user;
      return await query.FirstAsync();
    }

    public async Task<List<UserModel>> GetUsersForRole(string role)
    {
      var query =
        from u in Aimas.Users
        join ur in Aimas.UserRoles on u.Id equals ur.UserId
        join r in Aimas.Roles on ur.RoleId equals r.Id
        where r.Name == role
        select new UserModel()
        {
          Id = u.Id,
          Email = u.Email,
          FirstName = u.FirstName,
          LastName = u.LastName
        };
      return await query.ToListAsync();
    }

    #region UTIL

    public IdentityUserRole<long> NewIdentityRole(long usrrID, long roleID)
    {
      return new IdentityUserRole<long>() { UserId = usrrID, RoleId = roleID };
    }

    #endregion
  }
}
