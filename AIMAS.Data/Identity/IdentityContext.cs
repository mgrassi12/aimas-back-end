using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AIMAS.Data.Identity
{
  public class IdentityContext : IdentityDbContext<UserModel_DB, RoleModel_DB, Guid>
  {

    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

      modelBuilder.Entity<UserModel_DB>()
        .HasAlternateKey(user => user.UserName)
        .HasName("AK_AspNetUsers_UserName");

      modelBuilder.Entity<UserModel_DB>()
        .HasAlternateKey(user => user.Email)
        .HasName("AK_AspNetUsers_Email");

      modelBuilder.Entity<RoleModel_DB>()
        .HasAlternateKey(role => role.Name)
        .HasName("AK_AspNetRoles_Name");

      base.OnModelCreating(modelBuilder);
    }

  }

}
