using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("reservationEntry")]
  public class ReservationEntryModel_DB
    {
    [Key]
    public ReservationModel_DB Reservation { get; set; }

    [Key]
    public InventoryModel_DB Inventory { get; set; }

    public ReservationEntryModel_DB(ReservationModel_DB reservation, InventoryModel_DB inventory)
    {
      Reservation = reservation;
      Inventory = inventory;
    }
  }
}
