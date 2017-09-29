using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using AIMAS.API.Models;

namespace AIMAS.API.Providers
{
  public class EmailNotificationProvider : INotificationProvider
  {

    string Host { get; }
    int Port { get; }
    bool SSL { get; }
    string UserName { get; }
    string Password { get; }

    public EmailNotificationProvider(IConfiguration configuration)
    {
      var emailConfig = configuration.GetSection("Email");
      Host = emailConfig.GetValue<string>("Host");
      Port = emailConfig.GetValue<int>("Port");
      SSL = emailConfig.GetValue<bool>("SSL");
      UserName = emailConfig.GetValue<string>("UserName");
      Password = emailConfig.GetValue<string>("Password");
    }
    public bool SendMessage(NotificationMessage message)
    {
      return SendEmail(message);
    }

    private bool SendEmail(NotificationMessage msg)
    {
      try
      {
        var client = GetClient();
        var email = GetMessage(msg);
        client.Send(email);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private SmtpClient GetClient()
    {
      return new SmtpClient()
      {
        Host = Host,
        Port = Port,
        EnableSsl = SSL,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(UserName, Password)
      };
    }

    private MailMessage GetMessage(NotificationMessage msg)
    {
      return new MailMessage("no_reply_AIMAS@gmail.com", msg.To, msg.Subject, msg.Body);
    }
  }
}
