using AIMAS.Data.Inventory;
using System;

namespace AIMAS.Data.Models
{
  public class ChangeEventModel : IAimasModel<ChangeEventModel_DB>
  {
    public InventoryModel Inventory { get; set; }
    public long ID { get; set; }
    public UserModel User { get; set; }
    public DateTime ChangeTime { get; set; }
    public string ChangeType { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }

    public ChangeEventModel(InventoryModel inventory, UserModel user, DateTime changeTime, string changeType, string oldValue, string newValue, long id = default)
    {
      Inventory = inventory;
      ID = id;
      User = user;
      ChangeTime = changeTime;
      ChangeType = changeType;
      OldValue = oldValue;
      NewValue = newValue;
    }

    public ChangeEventModel_DB CreateNewDbModel(AimasContext aimas)
    {
      var dbInventory = aimas.GetDbInventory(Inventory);
      var dbUser = aimas.GetDbUser(User);
      return new ChangeEventModel_DB(inventory: dbInventory, id: ID, user: dbUser, changeTime: ChangeTime, changeType: ChangeType, oldValue: OldValue, newValue: NewValue);
    }
  }
}
