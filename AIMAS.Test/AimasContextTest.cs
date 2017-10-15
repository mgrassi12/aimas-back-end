using AIMAS.Data;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using AIMAS.Data.Models;

namespace AIMAS.Test
{
  [TestClass]
  public class AimasContextTest
  {
    private AimasContext _aimas;
    private IdentityDB _identityDb;
    private InventoryDB _inventoryDb;

    [TestInitialize]
    public void TestInitialize()
    {
      _aimas = GetAimasContext();
      _identityDb = GetIdentityDb(_aimas);
      _inventoryDb = GetInventoryDb(_aimas);
      SetupDb(_aimas, _identityDb, _inventoryDb);
    }

    private static AimasContext GetAimasContext()
    {
      var options = new DbContextOptionsBuilder<AimasContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
      return new AimasContext(options);
    }

    private static IdentityDB GetIdentityDb(AimasContext context)
    {
      var manager = new UserManager<UserModel_DB>(new UserStore<UserModel_DB, RoleModel_DB, AimasContext, long>(context), null, new PasswordHasher<UserModel_DB>(), null, null, null, null, null, null);
      return new IdentityDB(context, manager);
    }

    private static InventoryDB GetInventoryDb(AimasContext context)
    {
      return new InventoryDB(context);
    }

    private static void SetupDb(AimasContext aimas, IdentityDB identity, InventoryDB inventory)
    {
      aimas.Database.EnsureCreated();
      identity.Initialize();
      inventory.Initialize();
    }

    #region UserTests
    [TestMethod]
    public void GetUserSuccessfully()
    {
      var result = _identityDb.GetUserAsync("test1@test.com");
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetUserWithMatchingEmail()
    {
      var result = _identityDb.GetUserAsync("admin@example.com").Result;
      Assert.AreEqual("admin@example.com", result.Email);
    }

    [TestMethod]
    public void AddUserSuccessfully()
    {
      var newUser = new UserModel_DB("Test", "1", "test1@test.com", "User");
      var result = _identityDb.CreateUserAsync(newUser, "Test").Result;
      Assert.IsTrue(result.Success);
      Assert.AreEqual(newUser, _aimas.Users.Find(newUser.Id));
    }
    #endregion

    #region InventoryTests
    [TestMethod]
    public void AddInventorySuccessfully()
    {
      //TODO: Currently dependent on Initialize data of method
      var dbLocation = _aimas.Locations.First();
      var inventory = new InventoryModel("Test", DateTime.Now, 10, dbLocation.ToModel());
      Assert.AreEqual(3, _aimas.Inventories.Count());

      _inventoryDb.AddInventory(inventory);
      Assert.AreEqual(4, _aimas.Inventories.Count());
      Assert.AreEqual(inventory.Name, _aimas.Inventories.Last().Name);
    }

    [TestMethod]
    public void GetExpiredInventorySuccessfully()
    {
      var dbLocation = _aimas.Locations.First();
      var inventory = new InventoryModel_DB("Test", DateTime.Now.AddDays(-10), 10, dbLocation);
      Assert.AreEqual(0, _inventoryDb.GetExpiredInventory().Count);

      _aimas.Inventories.Add(inventory);
      _aimas.SaveChanges();
      Assert.AreEqual(1, _inventoryDb.GetExpiredInventory().Count);
    }

    [TestMethod]
    public void GetExpiredInventory_ExcludeDisposedItemsCorrectly()
    {
      var dbLocation = _aimas.Locations.First();
      var dbUser = _aimas.Users.First();
      var inventory = new InventoryModel_DB("Test", DateTime.Now.AddDays(-10), 10, dbLocation);
      var report = new ReportModel_DB(inventory, ReportType.ExpirationDisposal, dbUser, DateTime.Now, dbUser, DateTime.Now);
      Assert.AreEqual(0, _inventoryDb.GetExpiredInventory().Count);

      _aimas.Inventories.Add(inventory);
      _aimas.SaveChanges();
      _aimas.Reports.Add(report);
      _aimas.SaveChanges();
      Assert.AreEqual(0, _inventoryDb.GetExpiredInventory().Count);
    }

    [TestMethod]
    public void RemoveInventorySuccessfully()
    {
      var inventory = _aimas.Inventories.First();
      Assert.IsTrue(_aimas.Inventories.ToList().Contains(inventory));

      _inventoryDb.RemoveInventory(inventory.ID);
      Assert.IsFalse(_aimas.Inventories.ToList().Contains(inventory));
    }
    #endregion

    #region AlertTimeInventoryTests
    private void GetUpcomingAlertTimes_TestSetUp(InventoryModel_DB inventory, IEnumerable<InventoryAlertTimeModel_DB> alertTimes)
    {
      _aimas.Inventories.Add(inventory);
      _aimas.SaveChanges();

      foreach (var alert in alertTimes)
        _aimas.InventoryAlertTimes.Add(alert);
      _aimas.SaveChanges();
    }

    [TestMethod]
    public void GetUpcomingExpiryAlertTimeSuccessfully()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(10), 10, _aimas.Locations.First());
      var alertTime = new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 15, inventory);
      GetUpcomingAlertTimes_TestSetUp(inventory, new[] { alertTime });

      var result = _inventoryDb.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(1, result.Count);
      Assert.AreEqual(alertTime, result[0]);
    }

    [TestMethod]
    public void NotGetUpcomingExpiryAlertTimeThatHasBeenSent()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(10), 10, _aimas.Locations.First());
      var alertTime = new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 15, inventory);
      alertTime.SentTime = DateTime.Now;
      GetUpcomingAlertTimes_TestSetUp(inventory, new[] { alertTime });

      var result = _inventoryDb.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void NotGetUpcomingExpiryAlertTimeWhenItemIsExpired()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(-10), 10, _aimas.Locations.First());
      var alertTime = new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 15, inventory);
      GetUpcomingAlertTimes_TestSetUp(inventory, new[] { alertTime });

      var result = _inventoryDb.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void NotGetUpcomingExpiryAlertTimeWhenItemIsDisposed()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(10), 10, _aimas.Locations.First());
      var alertTime = new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 15, inventory);
      GetUpcomingAlertTimes_TestSetUp(inventory, new[] { alertTime });

      var user = _aimas.Users.First();
      _aimas.Reports.Add(new ReportModel_DB(inventory, ReportType.ExpirationDisposal, user, DateTime.Now, user, DateTime.Now));
      _aimas.SaveChanges();

      var result = _inventoryDb.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(0, result.Count);
    }
    #endregion

  }
}
