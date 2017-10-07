using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
      // Roles
      Roles.AllRoles.ForEach(item => Aimas.Roles.Add(new RoleModel_DB(item)));
      // Save Changes
      Aimas.SaveChanges();
      // Admin User
      var adminUser = new UserModel_DB("Admin", "Admin", "admin@example.com", "Admin");
      var result = CreateUserAsync(adminUser, "Admin@1").Result;
      if (result.Success)
      {
        AddUserRoleAsync(adminUser, Roles.Admin).Wait();
      }
      else
      {
        throw new Exception($"Failed to Create Admin User {result.ErrorMessage}");
      }
      var testUser = new UserModel_DB("Test", "User", "aimasbackend@gmail.com", "Test");
      var result2 = CreateUserAsync(testUser, "Testuser@1").Result;
      if (result2.Success)
      {
        AddUserRoleAsync(testUser, Roles.User).Wait();
      }
      else
      {
        throw new Exception($"Failed to Create Test User {result2.ErrorMessage}");
      }
    }

    #region UserOperations
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

    public async Task<List<UserModel>> GetUsersAsync()
    {
      var query = from user in Aimas.Users
                  select user.ToModel();
      return await query.ToListAsync();
    }

    public async Task<UserModel> GetUserAsync(string email)
    {
      var query = from user in Aimas.Users
                  where user.Email == email
                  select user.ToModel();
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

    public async Task UpdateUser(UserModel user)
    {
      var result = Aimas.Users.Find(user.Id);
      result.UpdateDb(user, Aimas);
      await UpdateUserRoles(user, result);
      Aimas.SaveChanges();
    }

    public void RemoveUser(long id)
    {
      var user = Aimas.Users.Find(id);
      Aimas.Users.Remove(user);
      Aimas.SaveChanges();
    }
    #endregion

    #region RoleOperations
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

    public IdentityUserRole<long> NewIdentityRole(long userID, long roleID)
    {
      return new IdentityUserRole<long>() { UserId = userID, RoleId = roleID };
    }

    private async Task UpdateUserRoles(UserModel user, UserModel_DB userDb)
    {
      var userRoles = user.UserRoles.Select(x => x.Name).ToList();
      var userDBRoles = (await Manager.GetRolesAsync(userDb)).ToList();
      var toAdd = userRoles.Except(userDBRoles).ToList();
      var toRemove = userDBRoles.Except(userRoles).ToList();

      await Manager.AddToRolesAsync(userDb, toAdd);
      await Manager.RemoveFromRolesAsync(userDb, toRemove);
    }

    public async Task<List<RoleModel>> GetRolesAsync()
    {
      var query =
        from r in Aimas.Roles
        select r.ToModel();
      return await query.ToListAsync();
    }
    #endregion

    #region TimeLogOperations
    public void AddTimeLog(TimeLogModel log)
    {
      var dbLog = log.ToDbModel();
      dbLog.User = Aimas.GetDbUser(log.User);

      if (dbLog.ID == default)
        dbLog.ID = Aimas.GetNewIdForTimeLog();

      Aimas.TimeLogs.Add(dbLog);
      Aimas.SaveChanges();
    }

    public async Task<List<TimeLogModel>> GetTimeLogsAsync()
    {
      var query = from log in Aimas.TimeLogs
                  select log.ToModel();
      return await query.ToListAsync();
    }

    public async Task<List<TimeLogModel>> GetTimeLogsAsync(UserModel user)
    {
      var query = from log in Aimas.TimeLogs
                  where log.User == Aimas.GetDbUser(user)
                  select log.ToModel();
      return await query.ToListAsync();
    }

    public void UpdateTimeLog(TimeLogModel log)
    {
      var result = Aimas.TimeLogs.Find(log.ID);
      result.UpdateDb(log, Aimas);
      Aimas.SaveChanges();
    }

    public void RemoveTimeLog(int id)
    {
      var log = Aimas.TimeLogs.Find((long)id);
      Aimas.TimeLogs.Remove(log);
      Aimas.SaveChanges();
    }
    #endregion
  }
}
