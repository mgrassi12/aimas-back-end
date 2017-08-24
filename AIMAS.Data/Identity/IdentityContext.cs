using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace AIMAS.Data.Identity
{
  public class IdentityContext : IdentityDbContext<User, Role, int>
  {

    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }


    public static void Initialize(IdentityContext context)
    {
      //context.Database.Migrate();

      if (!context.Roles.AnyAsync().Result)
      {
        // Admin Role
        context.Roles.Add(new Role() { Name = "Admin" });
        // User Role
        context.Roles.Add(new Role() { Name = "User" });
        context.SaveChanges();
      }

      if (!context.Users.AnyAsync().Result)
      {
        // Admin User
        context.Users.Add(new User() { UserName = "Admin" });
        context.SaveChanges();

        // Add Admin Role To Admin User
        context.UserRoles.Add(new IdentityUserRole<int>()
        {
          RoleId = context.Roles.FirstAsync(role => role.Name == "Admin").Result.Id,
          UserId = context.Users.FirstAsync(user => user.UserName == "Admin").Result.Id
        });
        context.SaveChanges();
      }
    }
  }


  public class User : IdentityUser<int>
  {

  }


  public class Role : IdentityRole<int>
  {

  }
}
