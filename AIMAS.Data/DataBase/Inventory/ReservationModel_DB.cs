using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AIMAS.Data.Identity;
using AIMAS.Data.Models;

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

    public ReservationModel_DB(UserModel_DB user, InventoryModel_DB inventory, DateTime start, DateTime end, long id = default) : this()
    {
      ID = id;
      User = user;
      Inventory = inventory;
      BookingStart = start;
      BookingEnd = end;
    }

    public ReservationModel ToModel()
    {
      return new ReservationModel(id: ID, user: User.ToModel(), inventory: Inventory.ToModel(), start: BookingStart, end: BookingEnd);
    }
  }
}
