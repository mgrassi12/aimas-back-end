using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("inventory")]
  public class InventoryModel_DB
  {
    [Key]
    public int ID { get; set; }
    [Required, Column(TypeName = "varchar(50)")]
    public string Name { get; set; }
  }
}
