using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIMAS.API.Models
{
  public class NotificationMessage
  {
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public NotificationMessage()
    {

    }
    public NotificationMessage(string subject, string body)
    {
      Subject = subject;
      Body = body;
    }
    public NotificationMessage(string to, string subject, string body)
    {
      To = to;
      Subject = subject;
      Body = body;
    }
  }
}
