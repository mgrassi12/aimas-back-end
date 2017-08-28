using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AIMAS.Data.Identity;

namespace AIMAS.Data.Inventory
{
  [Table("reservation")]
  public class ReservationModel_DB
  {
    [Key]
    public long ID { get; set; }

    [Required]
    public UserModel_DB User { get; set; }

    [Required]
    public InventoryModel_DB Inventory { get; set; }

    [Required, DataType(DataType.DateTime)]
    public DateTime BookingStart { get; set; }

    [Required, DataType(DataType.DateTime)]
    public DateTime BookingEnd { get; set; }


    public ReservationModel_DB() : base()
    {

    }

    public ReservationModel_DB(UserModel_DB user, InventoryModel_DB inventory, DateTime start, DateTime end, int id = 0) : this()
    {
      ID = id;
      User = user;
      Inventory = inventory;
      BookingStart = start;
      BookingEnd = end;
    }
  }
}
