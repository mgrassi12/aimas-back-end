using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AIMAS.Data.Models;

namespace AIMAS.Data.Inventory
{
  [Table("location")]
  public class LocationModel_DB
  {
    [Key]
    public long ID { get; set; }

    [Required]
    public string Name { get; set; }


    public string Description { get; set; }

    public LocationModel_DB() : base()
    {

    }

    public LocationModel_DB(string name, string description = default, long id = default) : this()
    {
      ID = id;
      Name = name;
      Description = description;
    }

    public LocationModel TOModel()
    {
      return new LocationModel(id: ID, name: Name, description: Description);
    }
  }
}
