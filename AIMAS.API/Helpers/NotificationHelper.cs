using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIMAS.Data.Identity;
using AIMAS.Data.Models;
using AIMAS.API.Providers;
using AIMAS.API.Models;

namespace AIMAS.API.Helpers
{
  public class NotificationHelper
  {
    private EmailNotificationProvider EmailProvider { get; }

    public NotificationHelper(EmailNotificationProvider emailProvider)
    {
      EmailProvider = emailProvider;
    }

    public void SendNotificationToUser(UserModel user, NotificationMessage message)
    {
      SendEmailNotification(user.Email, message);
    }

    public void SendEmailNotification(string address, NotificationMessage message)
    {
      var msg = new NotificationMessage(address, message.Subject, message.Body);
      EmailProvider.SendMessage(msg);
    }


  }
}
