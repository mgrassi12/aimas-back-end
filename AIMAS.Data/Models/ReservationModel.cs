using System;
using System.Collections.Generic;
using System.Text;
using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class ReservationModel
  {
    public long ID { get; set; }
    public UserModel User { get; set; }
    public InventoryModel Inventory { get; set; }
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnd { get; set; }

    public ReservationModel()
    {

    }

    public ReservationModel(UserModel user, InventoryModel inventory, DateTime start, DateTime end, long id = default) : this()
    {
      ID = id;
      User = user;
      Inventory = inventory;
      BookingStart = start;
      BookingEnd = end;
    }

    public ReservationModel_DB ToDBModel()
    {
      return new ReservationModel_DB(id: ID, user: User.ToDBModel(), inventory: Inventory.ToDBModel(), start: BookingStart, end: BookingEnd);
    }
  }
}
