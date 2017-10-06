using System;
using System.Collections.Generic;

namespace AIMAS.Data.Identity
{
  public static class Roles
    {
    public const string Admin = "Admin";
    public const string InventoryManager = "InventoryManager";
    public const string User = "User";
    public static readonly List<string> AllRoles = new List<String> { Admin, InventoryManager, User};
  }
}
