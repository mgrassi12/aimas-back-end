using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public LocationModel_DB(string name, string description = null, int id = 0) : this()
    {
      ID = id;
      Name = name;
      Description = description;
    }
  }
}
