using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("reservationIdentity")]
  public class ReservationInventoryModel_DB
  {
    public long ReservationID { get; set; }
    public ReservationModel_DB Reservation { get; set; }

    public long InventoryID { get; set; }
    public InventoryModel_DB Inventory { get; set; }

    private ReservationInventoryModel_DB()
    {
    }

    public ReservationInventoryModel_DB(long reservation, long inventory)
    {
      ReservationID = reservation;
      InventoryID = inventory;
    }

    public ReservationInventoryModel_DB(ReservationModel_DB reservation, InventoryModel_DB inventory) : this(reservation.ID, inventory.ID)
    {
      Reservation = reservation;
      Inventory = inventory;
    }
  }
}
