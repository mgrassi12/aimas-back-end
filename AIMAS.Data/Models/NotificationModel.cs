using AIMAS.Data.Inventory;
using System;

namespace AIMAS.Data.Models
{
  public class NotificationModel : IAimasModel<NotificationModel_DB>
  {
    public UserModel User { get; set; }
    public InventoryModel Inventory { get; set; }
    public long ID { get; set; }
    public DateTime AlertDate { get; set; }
    public DateTime UpcomingEventDate { get; set; }
    
    public NotificationModel(UserModel user, InventoryModel inventory, DateTime alertDate, DateTime upcomingEventDate, long id = default)
    {
      User = user;
      Inventory = inventory;
      ID = id;
      AlertDate = alertDate;
      UpcomingEventDate = upcomingEventDate;
    }

    public NotificationModel_DB ToDbModel()
    {
      return new NotificationModel_DB(user: User.ToDbModel(), inventory: Inventory.ToDbModel(), id: ID, alertDate: AlertDate, upcomingEventDate: UpcomingEventDate);
    }
  }
}
