using System.Linq;
using AIMAS.API.Controllers;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMAS.Test.ControllerTests
{
  [TestClass]
  public class UtilControllerTests : AimasTestBase
  {
    [TestMethod]
    public void AddLocationSuccessfully()
    {
      var controller = new UtilController(InventoryDb);
      Assert.AreEqual(0, Aimas.Locations.Count());

      var location = new LocationModel("Test");
      var result = controller.AddLocation(location);
      Assert.IsTrue(result.Success);
      Assert.AreEqual(1, Aimas.Locations.Count());
      Assert.AreEqual("Test", Aimas.Locations.First().Name);
    }

    [TestMethod]
    public void GetLocationsSuccessfully()
    {
      var controller = new UtilController(InventoryDb);
      var result = controller.GetLocations();
      Assert.IsTrue(result.Success);
      Assert.AreEqual(0, result.ReturnObj.Count);

      AddTestLocation("Test 1", "Description 1");
      AddTestLocation("Second test", "Other");
      AddTestLocation("Other", "Second description");

      result = controller.GetLocations();
      Assert.IsTrue(result.Success);
      Assert.AreEqual(3, result.ReturnObj.Count);
    }

    [TestMethod]
    public void SearchLocationsOnNameSuccessfully()
    {
      var controller = new UtilController(InventoryDb);
      AddTestLocation("Test 1", "Description 1");
      AddTestLocation("Second test", "Other");
      AddTestLocation("Other", "Second description");

      var search = new LocationSearch { Name = "TEST" };
      var result = controller.GetLocations(search).ReturnObj;
      Assert.AreEqual(2, result.Count);
      result.ForEach(l => Assert.IsTrue(l.Name.ToUpper().Contains("TEST")));
    }

    [TestMethod]
    public void SearchLocationsOnDescriptionSuccessfully()
    {
      var controller = new UtilController(InventoryDb);
      AddTestLocation("Test 1", "Description 1");
      AddTestLocation("Second test", "Other");
      AddTestLocation("Other", "Second description");

      var search = new LocationSearch { Description = "DESCRIPTION" };
      var result = controller.GetLocations(search).ReturnObj;
      Assert.AreEqual(2, result.Count);
      result.ForEach(l => Assert.IsTrue(l.Description.ToUpper().Contains("DESCRIPTION")));
    }

    [TestMethod]
    public void SearchLocationsOnNameAndDescriptionSuccessfully()
    {
      var controller = new UtilController(InventoryDb);
      AddTestLocation("Test 1", "Description 1");
      AddTestLocation("Second test", "Other");
      AddTestLocation("Other", "Second description");

      var search = new LocationSearch { Name = "TEST", Description = "DESCRIPTION" };
      var result = controller.GetLocations(search).ReturnObj;
      Assert.AreEqual(1, result.Count);
      Assert.AreEqual("Test 1", result[0].Name);
      Assert.AreEqual("Description 1", result[0].Description);
    }

    [TestMethod]
    public void SearchLocations_GetCorrectPageResultObj()
    {
      var controller = new UtilController(InventoryDb);
      AddTestLocation("Test 1", "Description 1");
      AddTestLocation("Second test", "Other");
      AddTestLocation("Other", "Second description");

      var search = new LocationSearch { PageIndex = 2, PageSize = 1 };
      var result = controller.GetLocations(search);
      Assert.IsTrue(result.Success);
      Assert.AreEqual(1, result.ReturnObj.Count);
      Assert.AreEqual(3, result.TotalCount);
      Assert.AreEqual(2, result.PageIndex);
      Assert.AreEqual(1, result.PageSize);
    }

    [TestMethod]
    public void UpdateLocationSuccessfully()
    {
      var controller = new UtilController(InventoryDb);
      var location = new LocationModel_DB("Test", "Description");
      Aimas.Locations.Add(location);
      Aimas.SaveChanges();
      
      var result = controller.UpdateLocation(new LocationModel("Test 2", "Description 2", location.ID));
      Assert.IsTrue(result.Success);
      Assert.AreEqual("Test 2", location.Name);
      Assert.AreEqual("Description 2", location.Description);
    }

    [TestMethod]
    public void RemoveLocationSuccessfully()
    {
      var controller = new UtilController(InventoryDb);
      var location = new LocationModel_DB("Test");
      Aimas.Locations.Add(location);
      Aimas.SaveChanges();
      Assert.AreEqual(1, Aimas.Locations.Count());

      var result = controller.RemoveLocation(location.ID);
      Assert.IsTrue(result.Success);
      Assert.AreEqual(0, Aimas.Locations.Count());
    }
  }
}
