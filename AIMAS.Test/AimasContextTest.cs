using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using AIMAS.Data;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;

namespace AIMAS.Test
{
  [TestClass]
  public class AimasContextTest
  {

    private SqliteConnection Connection { get; set; }

    private AimasContext GetAimasContext()
    {
      if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
        Connection.Close();

      Connection = new SqliteConnection("DataSource=:memory:");
      Connection.Open();

      var options = new DbContextOptionsBuilder<AimasContext>()
                .UseSqlite(Connection)
                .Options;
      return new AimasContext(options);
    }

    private IdentityDB GetIdentityDB(AimasContext context)
    {
      var manager = new UserManager<UserModel_DB>(new UserStore<UserModel_DB, RoleModel_DB, AimasContext, long>(context), null, new PasswordHasher<UserModel_DB>(), null, null, null, null, null, null);
      return new IdentityDB(context, manager);
    }

    private InventoryDB GetInventoryDB(AimasContext context)
    {
      return new InventoryDB(context);
    }

    private void SetupDB(AimasContext context, IdentityDB identity, InventoryDB inventory)
    {
      context.Database.EnsureCreated();
      identity.Initialize();
      inventory.Initialize();
    }

    [TestMethod]
    public void TestInilizeDB()
    {
      var aimas = GetAimasContext();
      var identity = GetIdentityDB(aimas);
      var inventory = GetInventoryDB(aimas);
      SetupDB(aimas, identity, inventory);
    }

    [TestMethod]
    public void TestGetAdminUser()
    {
      var aimas = GetAimasContext();
      var identity = GetIdentityDB(aimas);
      var inventory = GetInventoryDB(aimas);
      SetupDB(aimas, identity, inventory);

      var result = identity.GetUserDBAsync("admin@example.com").Result;

      Assert.AreEqual(result.Email, "admin@example.com");
    }
  }
}
