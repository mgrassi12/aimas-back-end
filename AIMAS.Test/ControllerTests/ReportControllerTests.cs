using System;
using AIMAS.API.Controllers;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMAS.Test.ControllerTests
{
  [TestClass]
  public class ReportControllerTests : AimasTestBase
  {
    private void AddTestReport(InventoryModel_DB inventory, ReportType type, UserModel_DB user)
    {
      var report = new ReportModel_DB(inventory, type, user, DateTime.UtcNow, user, DateTime.UtcNow);
      Aimas.Reports.Add(report);
      Aimas.SaveChanges();
    }

    [TestMethod]
    public void SearchReportsOnInventoryIdSuccessfully()
    {
      var controller = new ReportController(InventoryDb);
      var item1 = AddTestInventory();
      var item2 = AddTestInventory();
      var user = AddTestUser();

      AddTestReport(item1, ReportType.ExpirationDisposal, user);
      AddTestReport(item1, ReportType.General, user);
      AddTestReport(item2, ReportType.ExpirationDisposal, user);

      var search = new ReportSearch { InventoryId = item1.ID };
      var result = controller.GetReports(search).ReturnObj;
      Assert.AreEqual(2, result.Count);
      result.ForEach(r => Assert.AreEqual(item1.ID, r.Inventory.ID));
    }

    [TestMethod]
    public void SearchReportsOnReportTypeSuccessfully()
    {
      var controller = new ReportController(InventoryDb);
      var item1 = AddTestInventory();
      var item2 = AddTestInventory();
      var user = AddTestUser();

      AddTestReport(item1, ReportType.ExpirationDisposal, user);
      AddTestReport(item1, ReportType.General, user);
      AddTestReport(item2, ReportType.ExpirationDisposal, user);

      var search = new ReportSearch { Type = ReportType.ExpirationDisposal};
      var result = controller.GetReports(search).ReturnObj;
      Assert.AreEqual(2, result.Count);
      result.ForEach(r => Assert.AreEqual(ReportType.ExpirationDisposal, r.Type));
    }

    [TestMethod]
    public void SearchReportsOnInventoryIdAndReportTypeSuccessfully()
    {
      var controller = new ReportController(InventoryDb);
      var item1 = AddTestInventory();
      var item2 = AddTestInventory();
      var user = AddTestUser();

      AddTestReport(item1, ReportType.ExpirationDisposal, user);
      AddTestReport(item1, ReportType.General, user);
      AddTestReport(item2, ReportType.ExpirationDisposal, user);

      var search = new ReportSearch { InventoryId = item1.ID, Type = ReportType.ExpirationDisposal };
      var result = controller.GetReports(search).ReturnObj;
      Assert.AreEqual(1, result.Count);
      Assert.AreEqual(item1.ID, result[0].Inventory.ID);
      Assert.AreEqual(ReportType.ExpirationDisposal, result[0].Type);
    }
  }
}
