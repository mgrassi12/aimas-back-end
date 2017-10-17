using AIMAS.Data.Identity;
using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("changeEvent")]
  public class ChangeEventModel_DB
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

    private const string NullValue = "NULL";

    private ChangeEventModel_DB()
    {
    }

    public ChangeEventModel_DB(InventoryModel_DB inventory, UserModel_DB user, string changeType, string oldValue, string newValue)
    {
      Inventory = inventory;
      User = user;
      ChangeTime = DateTime.UtcNow;
      ChangeType = changeType;
      OldValue = oldValue ?? NullValue;
      NewValue = newValue ?? NullValue;
    }
  }
}
