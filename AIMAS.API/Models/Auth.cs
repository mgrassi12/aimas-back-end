using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIMAS.API.Models
{
  public class CurrentUserInfo
  {
    public bool IsAuth { get; set; }
    public bool IsAdmin { get; set; }
    public string Role { get; set; }
  }
}
