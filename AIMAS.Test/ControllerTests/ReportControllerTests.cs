using System;
using System.Linq;
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
    public void AddReportSuccessfully()
    {
      var controller = new ReportController(InventoryDb);
      var item = AddTestInventory();
      var user = AddTestUser();
      Assert.AreEqual(0, Aimas.Reports.Count());

      var report = new ReportModel(item.ToModel(), ReportType.ExpirationDisposal, user.ToModel(), DateTime.UtcNow, user.ToModel(), DateTime.UtcNow);
      var result = controller.AddReport(report);
      Assert.IsTrue(result.Success);
      Assert.AreEqual(1, Aimas.Reports.Count());

      var reportDb = Aimas.Reports.First();
      Assert.AreEqual(item, reportDb.Inventory);
      Assert.AreEqual(ReportType.ExpirationDisposal, reportDb.Type);
    }

    [TestMethod]
    public void GetReportsSuccessfully()
    {
      var controller = new ReportController(InventoryDb);
      var item1 = AddTestInventory();
      var item2 = AddTestInventory();
      var user = AddTestUser();

      var result = controller.GetReports();
      Assert.IsTrue(result.Success);
      Assert.AreEqual(0, result.ReturnObj.Count);

      AddTestReport(item1, ReportType.ExpirationDisposal, user);
      AddTestReport(item1, ReportType.General, user);
      AddTestReport(item2, ReportType.ExpirationDisposal, user);

      result = controller.GetReports();
      Assert.IsTrue(result.Success);
      Assert.AreEqual(3, result.ReturnObj.Count);
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

      var search = new ReportSearch { InventoryName = item1.Name };
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

      var search = new ReportSearch { Type = ReportType.ExpirationDisposal };
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

      var search = new ReportSearch { InventoryName = item1.Name, Type = ReportType.ExpirationDisposal };
      var result = controller.GetReports(search).ReturnObj;
      Assert.AreEqual(1, result.Count);
      Assert.AreEqual(item1.ID, result[0].Inventory.ID);
      Assert.AreEqual(ReportType.ExpirationDisposal, result[0].Type);
    }

    [TestMethod]
    public void SearchReports_GetCorrectPageResultObj()
    {
      var controller = new ReportController(InventoryDb);
      var item1 = AddTestInventory();
      var item2 = AddTestInventory();
      var user = AddTestUser();

      AddTestReport(item1, ReportType.ExpirationDisposal, user);
      AddTestReport(item1, ReportType.General, user);
      AddTestReport(item2, ReportType.ExpirationDisposal, user);

      var search = new ReportSearch { PageIndex = 2, PageSize = 1 };
      var result = controller.GetReports(search);
      Assert.IsTrue(result.Success);
      Assert.AreEqual(1, result.ReturnObj.Count);
      Assert.AreEqual(3, result.TotalCount);
      Assert.AreEqual(2, result.PageIndex);
      Assert.AreEqual(1, result.PageSize);
    }

    [TestMethod]
    public void RemoveReportSuccessfully()
    {
      var controller = new ReportController(InventoryDb);
      var item = AddTestInventory();
      var user = AddTestUser();

      var report = new ReportModel_DB(item, ReportType.ExpirationDisposal, user, DateTime.UtcNow, user, DateTime.UtcNow);
      Aimas.Reports.Add(report);
      Aimas.SaveChanges();
      Assert.AreEqual(1, Aimas.Reports.Count());

      var result = controller.RemoveReport(report.ID);
      Assert.IsTrue(result.Success);
      Assert.AreEqual(0, Aimas.Reports.Count());
    }
  }
}
