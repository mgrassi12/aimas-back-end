using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using AIMAS.Data.Models;

namespace AIMAS.Test
{
  [TestClass]
  public class AimasContextTest : AimasTestBase
  {
    #region UserTests
    [TestMethod]
    public void GetUsersSuccessfully()
    {
      var user1 = new UserModel_DB("Test1", "T", "test1@test.com", "T");
      var user2 = new UserModel_DB("Test2", "T", "test2@test.com", "T");
      AddTestUser(user1);
      AddTestUser(user2);

      var result = IdentityDb.GetUsersAsync().Result;
      Assert.AreEqual(2, result.Count);
      Assert.AreEqual(user1.Email, result[0].Email);
      Assert.AreEqual(user2.Email, result[1].Email);
    }

    [TestMethod]
    public void GetUserWithMatchingEmail()
    {
      var user = new UserModel_DB("Test1", "T", "test1@test.com", "T");
      AddTestUser(user);

      var result = IdentityDb.GetUserAsync(user.Email).Result;
      Assert.AreEqual(user.Email, result.Email);
    }

    [TestMethod]
    public void AddUserSuccessfully()
    {
      var newUser = new UserModel_DB("Test", "1", "test1@test.com", "User");
      var result = IdentityDb.CreateUserAsync(newUser, "Test").Result;
      Assert.IsTrue(result.Success);
      Assert.AreEqual(newUser, Aimas.Users.Find(newUser.Id));
    }
    #endregion

    #region InventoryTests
    [TestMethod]
    public void AddInventorySuccessfully()
    {
      var dbLocation = AddTestLocation("Test");
      var inventory = new InventoryModel("Test", DateTime.Now, 10, dbLocation.ToModel());
      Assert.AreEqual(0, Aimas.Inventories.Count());

      InventoryDb.AddInventory(inventory);
      Assert.AreEqual(1, Aimas.Inventories.Count());
      Assert.AreEqual(inventory.Name, Aimas.Inventories.Last().Name);
    }

    [TestMethod]
    public void GetExpiredInventorySuccessfully()
    {
      var dbLocation = AddTestLocation("Test");
      var inventory = new InventoryModel_DB("Test", DateTime.Now.AddDays(-10), 10, dbLocation);
      Assert.AreEqual(0, InventoryDb.GetExpiredInventory().Count);

      Aimas.Inventories.Add(inventory);
      Aimas.SaveChanges();
      Assert.AreEqual(1, InventoryDb.GetExpiredInventory().Count);
    }

    [TestMethod]
    public void GetExpiredInventory_ExcludeDisposedItemsCorrectly()
    {
      var dbLocation = AddTestLocation("Test");
      var dbUser = AddTestUser();
      var inventory = new InventoryModel_DB("Test", DateTime.Now.AddDays(-10), 10, dbLocation);
      var report = new ReportModel_DB(inventory, ReportType.ExpirationDisposal, dbUser, DateTime.Now, dbUser, DateTime.Now);
      Assert.AreEqual(0, InventoryDb.GetExpiredInventory().Count);

      Aimas.Inventories.Add(inventory);
      Aimas.SaveChanges();
      Aimas.Reports.Add(report);
      Aimas.SaveChanges();
      Assert.AreEqual(0, InventoryDb.GetExpiredInventory().Count);
    }

    [TestMethod]
    public void GetCriticalInventoryNotInDefaultLocationSuccessfully()
    {
      var currentLocation = AddTestLocation("Current");
      var defaultLocation = AddTestLocation("Default");

      var inventory = new InventoryModel_DB("Test", DateTime.Now.AddDays(10), 10, currentLocation, defaultLocation, isCritical:true);
      Assert.AreEqual(0, InventoryDb.GetCriticalInventoryNotInDefaultLocation().Count);

      Aimas.Inventories.Add(inventory);
      Aimas.SaveChanges();
      Assert.AreEqual(1, InventoryDb.GetCriticalInventoryNotInDefaultLocation().Count);
    }

    [TestMethod]
    public void GetCriticalInventory_ExcludeInventoryWithNoDefaultLocationCorrectly()
    {
      var currentLocation = AddTestLocation("Current");

      var inventory = new InventoryModel_DB("Test", DateTime.Now.AddDays(10), 10, currentLocation, defaultLocation: null, isCritical: true);
      Assert.AreEqual(0, InventoryDb.GetCriticalInventoryNotInDefaultLocation().Count);

      Aimas.Inventories.Add(inventory);
      Aimas.SaveChanges();
      Assert.AreEqual(0, InventoryDb.GetCriticalInventoryNotInDefaultLocation().Count);
    }

    [TestMethod]
    public void RemoveInventorySuccessfully()
    {
      var inventory = AddTestInventory();
      Assert.IsTrue(Aimas.Inventories.ToList().Contains(inventory));

      InventoryDb.RemoveInventory(inventory.ID);
      Assert.IsFalse(Aimas.Inventories.ToList().Contains(inventory));
    }
    #endregion

    #region AlertTimeInventoryTests
    private void GetUpcomingAlertTimes_TestSetUp(InventoryModel_DB inventory, IEnumerable<InventoryAlertTimeModel_DB> alertTimes)
    {
      Aimas.Inventories.Add(inventory);
      Aimas.SaveChanges();

      foreach (var alert in alertTimes)
        Aimas.InventoryAlertTimes.Add(alert);
      Aimas.SaveChanges();
    }

    [TestMethod]
    public void GetUpcomingExpiryAlertTimeSuccessfully()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(10), 10, AddTestLocation("Test"));
      var alertTime = new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 15, inventory);
      GetUpcomingAlertTimes_TestSetUp(inventory, new[] { alertTime });

      var result = InventoryDb.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(1, result.Count);
      Assert.AreEqual(alertTime, result[0]);
    }

    [TestMethod]
    public void NotGetUpcomingExpiryAlertTimeThatHasBeenSent()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(10), 10, AddTestLocation("Test"));
      var alertTime = new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 15, inventory);
      alertTime.SentTime = DateTime.Now;
      GetUpcomingAlertTimes_TestSetUp(inventory, new[] { alertTime });

      var result = InventoryDb.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void NotGetUpcomingExpiryAlertTimeWhenItemIsExpired()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(-10), 10, AddTestLocation("Test"));
      var alertTime = new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 15, inventory);
      GetUpcomingAlertTimes_TestSetUp(inventory, new[] { alertTime });

      var result = InventoryDb.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void NotGetUpcomingExpiryAlertTimeWhenItemIsDisposed()
    {
      var inventory = new InventoryModel_DB("Test Item", DateTime.Now.AddDays(10), 10, AddTestLocation("Test"));
      var alertTime = new InventoryAlertTimeModel_DB(InventoryAlertTimeType.Inventory_E_Date, 15, inventory);
      GetUpcomingAlertTimes_TestSetUp(inventory, new[] { alertTime });

      var user = AddTestUser();
      Aimas.Reports.Add(new ReportModel_DB(inventory, ReportType.ExpirationDisposal, user, DateTime.Now, user, DateTime.Now));
      Aimas.SaveChanges();

      var result = InventoryDb.GetUpcomingExpiryAlertTimes();
      result = result.Where(alert => alert.Inventory == inventory).ToList();
      Assert.AreEqual(0, result.Count);
    }
    #endregion

  }
}
