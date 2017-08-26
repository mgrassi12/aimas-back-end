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
          var role = await Identity.Roles.FirstAsync(r => r.Name == roleName);

          // Add UserRole
          await Identity.UserRoles.AddAsync(NewIdentityRole(user.Id, role.Id));
          Identity.SaveChanges();

          // Success
          result.Success = true;
        }
        else
        {
          // Failed to Create User
          result.Success = false;
          result.AddIdentityErrors(createUser.Errors);
        }
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      // Return Result;
      return result;
    }

    public List<UserModel> GetUsers()
    {
      var list = from user in Identity.Users
                 select new UserModel()
                 {
                   UserName = user.UserName,
                   Email = user.Email,
                   FirstName = user.FirstName,
                   LastName = user.FirstName
                 };
      return list.ToList();
    }

    #region UTIL

    public IdentityUserRole<Guid> NewIdentityRole(Guid usrrID, Guid roleID)
    {
      return new IdentityUserRole<Guid>() { UserId = usrrID, RoleId = roleID };
    }

    #endregion
  }
}
