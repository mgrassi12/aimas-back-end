using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using AIMAS.API.Helpers;
using AIMAS.API.Models;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using Microsoft.EntityFrameworkCore;

namespace AIMAS.API.Services
{
  public class NotificationService
  {
    private Timer Timer { get; }
    private NotificationHelper Helper { get; }
    private IdentityDB Identity { get; }
    private InventoryDB Inventory { get; }

    public NotificationService(NotificationHelper helper, IdentityDB identity, InventoryDB inventory)
    {
      Helper = helper;
      Identity = identity;
      Inventory = inventory;
      Timer = new Timer(60 * 1000);
      Timer.Elapsed += Timer_Elapsed;
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      CheckInventory();
    }

    public void Start()
    {
      Timer.Enabled = true;
      CheckInventory();
    }

    public void Stop()
    {
      Timer.Enabled = false;
    }

    private void CheckInventory()
    {
      //CheckExpiredInventory();
      CheckUpcomingAleryNotifications();
    }

    private void CheckExpiredInventory()
    {
      var items = Inventory.GetExpiredInventory();
      foreach (var item in items)
      {
        var msg = new NotificationMessage(
          $"Inventory Item Has Expired - {item.Name}",
          $"Name: {item.Name}" +
          $"Description: {item.Description}"
          );
        var users = Identity.GetUsersForRole(Roles.Admin).Result;
        foreach (var user in users)
        {
          Helper.SendNotificationToUser(user, msg);
        }

      }
    }

    private void CheckUpcomingAleryNotifications()
    {
      var list = Inventory.GetUpcomingExpiryAlertTimes();
    }


  }
}
