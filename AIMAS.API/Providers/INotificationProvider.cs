using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIMAS.API.Models;

namespace AIMAS.API.Providers
{
  interface INotificationProvider
  {
    bool SendMessage(NotificationMessage message);
  }
}
