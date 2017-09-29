using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

using AIMAS.Data;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;
using AIMAS.API.Providers;
using AIMAS.API.Models;

namespace AIMAS.Test
{
  [TestClass]
  public class AimasNotificationTest
  {
    [TestMethod]
    public void TestSendEmail()
    {
      var configBuilder = new ConfigurationBuilder();
      configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
      var config = configBuilder.Build();
      var emailProvider = new EmailNotificationProvider(config);
      var msg = new NotificationMessage("aimasbackend@gmail.com", "Test Email", "This is a Test Email");
      var success = emailProvider.SendMessage(msg);
      Assert.IsTrue(success);
    }
  }
}
