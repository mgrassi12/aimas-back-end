using AIMAS.Data.Identity;
using AIMAS.Data.Models;
using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("Notification")]
  public class NotificationModel_DB : IAimasDbModel<NotificationModel>
  {
    [Key]
    public UserModel_DB User { get; set; }

    [Key]
    public InventoryModel_DB Inventory { get; set; }

    [Key]
    public long ID { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime AlertDate { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime UpcomingEventDate { get; set; }

    public NotificationModel_DB(UserModel_DB user, InventoryModel_DB inventory, DateTime alertDate, DateTime upcomingEventDate, long id = default)
    {
      User = user;
      Inventory = inventory;
      ID = id;
      AlertDate = alertDate;
      UpcomingEventDate = upcomingEventDate;
    }

    public NotificationModel ToModel()
    {
      return new NotificationModel(user: User.ToModel(), inventory: Inventory.ToModel(), id: ID, alertDate: AlertDate, upcomingEventDate: UpcomingEventDate);
    }
  }
}
