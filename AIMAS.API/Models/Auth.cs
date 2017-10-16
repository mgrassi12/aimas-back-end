using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIMAS.Data.Models;

namespace AIMAS.API.Models
{
  public class CurrentUserInfo
  {
    public bool IsAuth { get; set; }
    public UserModel User { get; set; }
  }
}
