using AIMAS.Data.Identity;
using AIMAS.Data.Models;
using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("notification")]
  public class NotificationModel_DB : IAimasDbModel<NotificationModel>
  {
    [Required]
    public InventoryModel_DB Inventory { get; set; }

    [Required]
    public UserModel_DB User { get; set; }

    [Key]
    public long ID { get; set; }

    [Required]
    public string Type { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime AlertDate { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime UpcomingEventDate { get; set; }

    public NotificationModel_DB(InventoryModel_DB inventory, UserModel_DB user, string type, DateTime alertDate, DateTime upcomingEventDate, long id = default)
    {
      Inventory = inventory;
      User = user;
      ID = id;
      Type = type;
      AlertDate = alertDate;
      UpcomingEventDate = upcomingEventDate;
    }

    public NotificationModel ToModel()
    {
      return new NotificationModel(inventory: Inventory.ToModel(), user: User.ToModel(), id: ID, type: Type, alertDate: AlertDate, upcomingEventDate: UpcomingEventDate);
    }
  }
}
