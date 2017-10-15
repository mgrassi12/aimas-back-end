using System;
using System.Collections.Generic;
using System.Timers;
using AIMAS.API.Helpers;
using AIMAS.API.Models;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;

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
      CheckExpiredInventory();
      CheckInventoryNeedingMaintenance();
      CheckUpcomingAlertNotifications();
    }

    private void CheckExpiredInventory()
    {
      var items = Inventory.GetExpiredInventory();
      foreach (var item in items)
      {
        SendMessageToAdminUsers(new NotificationMessage(
          $"Inventory Item has expired - {item.Name}",
          $"Name: {item.Name}\n" +
          $"Description: {item.Description}\n")
          );
      }
    }

    private void CheckInventoryNeedingMaintenance()
    {
      var items = Inventory.GetInventoryNeedingMaintenance();
      foreach (var item in items)
      {
        SendMessageToAdminUsers(new NotificationMessage(
          $"Inventory Item needs maintenance - {item.Name}",
          $"Name: {item.Name}" +
          $"Description: {item.Description}")
        );
      }
    }

    private void CheckUpcomingAlertNotifications()
    {
      SendUpcomingAlertNotifications(Inventory.GetUpcomingExpiryAlertTimes(), item => item.ExpirationDate, "Expiration", "is about to expire");
      SendUpcomingAlertNotifications(Inventory.GetUpcomingMaintenanceAlertTimes(), item => item.GetMaintenanceDate(), "Maintenance", "needs maintenance");
    }

    private void SendMessageToAdminUsers(NotificationMessage msg)
    {
      var users = Identity.GetUsersForRole(Roles.Admin).Result;
      foreach (var user in users)
      {
        Helper.SendNotificationToUser(user, msg);
      }
    }

    private void SendUpcomingAlertNotifications(IEnumerable<InventoryAlertTimeModel_DB> alerts,
      Func<InventoryModel_DB, DateTime> getDate, string alertType, string alertTypeDescription)
    {
      foreach (var alert in alerts)
      {
        var date = getDate(alert.Inventory);
        SendMessageToAdminUsers(new NotificationMessage(
          $"Inventory Item {alertTypeDescription} in {(date - DateTime.Now).Days} days",
          $"Name: {alert.Inventory.Name}\n" +
          $"Description: {alert.Inventory.Description}\n" +
          $"{alertType} Date: {date.ToShortDateString()}\n")
        );
        alert.SentTime = DateTime.UtcNow;
      }
    }

  }
}
