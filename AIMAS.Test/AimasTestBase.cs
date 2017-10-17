using System;
using System.Security.Claims;
using System.Security.Principal;
using AIMAS.Data;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMAS.Test
{
  [TestClass]
  public class AimasTestBase
  {
    protected AimasContext Aimas;
    protected IdentityDB IdentityDb;
    protected InventoryDB InventoryDb;

    [TestInitialize]
    public void TestInitialize()
    {
      Aimas = GetAimasContext();
      Aimas.Database.EnsureCreated();
      IdentityDb = GetIdentityDb(Aimas);
      InventoryDb = new InventoryDB(Aimas);
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

    protected InventoryModel_DB AddTestInventory(string name = "Test")
    {
      var location = AddTestLocation("Test for Inventory");
      var inventory = new InventoryModel_DB(name, DateTime.UtcNow, 10, location);
      Aimas.Inventories.Add(inventory);
      Aimas.SaveChanges();
      return inventory;
    }

    protected LocationModel_DB AddTestLocation(string name = "Test", string description = null)
    {
      var location = new LocationModel_DB(name, description);
      Aimas.Locations.Add(location);
      Aimas.SaveChanges();
      return location;
    }

    protected UserModel_DB AddTestUser()
    {
      var user = new UserModel_DB("TestFirst", "TestLast", "TestEmail", "TestPosition");
      Aimas.Users.Add(user);
      Aimas.SaveChanges();
      return user;
    }

    protected void SetupControllerUser<T>(T controller, long id) where T : Controller
    {
      var identity = new GenericIdentity("TestUser");
      identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", id.ToString()));

      var fakePrincipal = A.Fake<ClaimsPrincipal>(op => op.Wrapping(new ClaimsPrincipal(identity)));

      controller.ControllerContext = A.Fake<ControllerContext>();
      controller.ControllerContext.HttpContext = A.Fake<HttpContext>();

      A.CallTo(() => controller.ControllerContext.HttpContext.User).Returns(fakePrincipal);
    }
  }
}
