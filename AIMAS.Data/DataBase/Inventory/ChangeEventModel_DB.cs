using AIMAS.Data.Identity;
using AIMAS.Data.Models;
using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("changeEvent")]
  public class ChangeEventModel_DB : IAimasDbModel<ChangeEventModel>
  {
    [Required]
    public InventoryModel_DB Inventory { get; set; }

    [Key]
    public long ID { get; set; }

    [Required]
    public UserModel_DB User { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime ChangeTime { get; set; }

    [Required]
    public string ChangeType { get; set; }

    [Required]
    public string OldValue { get; set; }

    [Required]
    public string NewValue { get; set; }

    private ChangeEventModel_DB()
    {
    }

    public ChangeEventModel_DB(InventoryModel_DB inventory, UserModel_DB user, DateTime changeTime, string changeType, string oldValue, string newValue, long id = default)
    {
      Inventory = inventory;
      ID = id;
      User = user;
      ChangeTime = changeTime;
      ChangeType = changeType;
      OldValue = oldValue;
      NewValue = newValue;
    }

    public ChangeEventModel ToModel()
    {
      return new ChangeEventModel(inventory: Inventory.ToModel(), id: ID, user: User.ToModel(), changeTime: ChangeTime, changeType: ChangeType, oldValue: OldValue, newValue: NewValue);
    }
  }
}
