using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AIMAS.Data.Models;
using AIMAS.Data.Util;
using NpgsqlTypes;

namespace AIMAS.Data.Inventory
{
  [Table("inventory")]
  public class InventoryModel_DB
  {
    [Key]
    public long ID { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime ExpirationDate { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime MaintanceDate { get; set; }

    [Required]
    public LocationModel_DB Location { get; set; }

    public InventoryModel_DB() : base()
    {

    }

    public InventoryModel_DB(string name, DateTime expire, DateTime maintance, LocationModel_DB location, string description = null, long id = 0) : this()
    {
      ID = id;
      Name = name;
      Description = description;
      ExpirationDate = expire;
      MaintanceDate = maintance;
      Location = location;
    }

    public InventoryModel ToModel()
    {
      return new InventoryModel(id: ID, name: Name, description: Description, expiration: ExpirationDate, maintance: MaintanceDate, location: Location.TOModel());
    }
  }
}
