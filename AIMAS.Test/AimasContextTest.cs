using AIMAS.Data;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AIMAS.Test
{
  [TestClass]
  public class AimasContextTest
  {
    private AimasContext Aimas;
    private IdentityDB Identity;
    private InventoryDB Inventory;

    [TestInitialize]
    public void TestInitialize()
    {
      Aimas = GetAimasContext();
      Identity = GetIdentityDb(Aimas);
      Inventory = GetInventoryDb(Aimas);
      SetupDB(Aimas, Identity, Inventory);
    }

    private AimasContext GetAimasContext()
    {
      var options = new DbContextOptionsBuilder<AimasContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
      return new AimasContext(options);
    }

    private IdentityDB GetIdentityDb(AimasContext context)
    {
      var manager = new UserManager<UserModel_DB>(new UserStore<UserModel_DB, RoleModel_DB, AimasContext, long>(context), null, new PasswordHasher<UserModel_DB>(), null, null, null, null, null, null);
      return new IdentityDB(context, manager);
    }

    private InventoryDB GetInventoryDb(AimasContext context)
    {
      return new InventoryDB(context);
    }

    private void SetupDB(AimasContext aimas, IdentityDB identity, InventoryDB inventory)
    {
      aimas.Database.EnsureCreated();
      identity.Initialize();
      inventory.Initialize();
    }

    #region UserTests
    [TestMethod]
    public void GetUserSuccessfully()
    {
      var result = Identity.GetUserAsync("test1@test.com");
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetUserWithMatchingEmail()
    {
      var result = Identity.GetUserAsync("admin@example.com").Result;
      Assert.AreEqual("admin@example.com", result.Email);
    }

    [TestMethod]
    public void AddUserSuccessfully()
    {
      var newUser = new UserModel_DB("Test", "1", "test1@test.com", "User");
      var result = Identity.CreateUserAsync(newUser, "Test").Result;
      Assert.IsTrue(result.Success);
      Assert.AreEqual(newUser, Aimas.Users.Find(newUser.Id));
    }
    #endregion

    #region AlertTimeInventoryTests
    private void GetUpcomingAlertTimes_TestSetUp(InventoryModel_DB inventory, InventoryAlertTimeModel_DB[] alertTimes)
    {
      Aimas.Inventories.Add(inventory);
      Aimas.SaveChanges();

      foreach (InventoryAlertTimeModel_DB alert in alertTimes)
        Aimas.InventoryAlertTimes.Add(alert);
      Aimas.SaveChanges();
    }

    [TestMethod]
    public void GetUpcomingExpiryAlertTimeSuccessfully()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(10), 10, Aimas.Locations.First());
      var alertTime = new InventoryAlertTimeModel_DB(inventory, AlertTimeType.Inventory_E_Date, 15);
      GetUpcomingAlertTimes_TestSetUp(inventory, new InventoryAlertTimeModel_DB[] {alertTime});

      var result = Inventory.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(1, result.Count());
      Assert.AreEqual(alertTime, result[0]);
    }

    [TestMethod]
    public void NotGetUpcomingExpiryAlertTimeThatHasBeenSent()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(10), 10, Aimas.Locations.First());
      var alertTime = new InventoryAlertTimeModel_DB(inventory, AlertTimeType.Inventory_E_Date, 15);
      alertTime.SentTime = DateTime.Now;
      GetUpcomingAlertTimes_TestSetUp(inventory, new InventoryAlertTimeModel_DB[] {alertTime});

      var result = Inventory.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(0, result.Count());
    }
    #endregion

  }
}
