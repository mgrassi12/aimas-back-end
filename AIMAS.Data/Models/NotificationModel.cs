using AIMAS.Data.Inventory;
using System;

namespace AIMAS.Data.Models
{
  public class NotificationModel : IAimasModel<NotificationModel_DB>
  {
    public InventoryModel Inventory { get; set; }
    public UserModel User { get; set; }
    public long ID { get; set; }
    public string Type { get; set; }
    public DateTime AlertDate { get; set; }
    public DateTime UpcomingEventDate { get; set; }
    
    public NotificationModel(InventoryModel inventory, UserModel user, string type, DateTime alertDate, DateTime upcomingEventDate, long id = default)
    {
      Inventory = inventory;
      User = user;
      ID = id;
      Type = type;
      AlertDate = alertDate;
      UpcomingEventDate = upcomingEventDate;
    }

    public NotificationModel_DB ToDbModel()
    {
      return new NotificationModel_DB(inventory: Inventory.ToDbModel(), user: User.ToDbModel(), id: ID, type: Type, alertDate: AlertDate, upcomingEventDate: UpcomingEventDate);
    }
  }
}
