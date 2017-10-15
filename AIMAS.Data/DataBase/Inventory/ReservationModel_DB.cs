using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using AIMAS.Data.Identity;
using AIMAS.Data.Models;
using AIMAS.Data.Util;

namespace AIMAS.Data.Inventory
{
  [Table("reservation")]
  public class ReservationModel_DB : IAimasDbModelWithUpdate<ReservationModel>
  {
    [Required]
    public UserModel_DB User { get; set; }

    [Key]
    public long ID { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime BookingStart { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime BookingEnd { get; set; }

    [Required]
    public string BookingPurpose { get; set; }

    [Required]
    public LocationModel_DB Location { get; set; }
    public List<ReservationInventoryModel_DB> ReservationInventories { get; set; }

    private ReservationModel_DB()
    {
    }

    public ReservationModel_DB(UserModel_DB user, DateTime start, DateTime end, string purpose, LocationModel_DB location, List<ReservationInventoryModel_DB> reservationInventories = default, long id = default)
    {
      ID = id;
      User = user;
      BookingStart = start;
      BookingEnd = end;
      BookingPurpose = purpose;
      Location = location;
      ReservationInventories = reservationInventories ?? new List<ReservationInventoryModel_DB>();
    }

    public ReservationModel ToModel()
    {
      return new ReservationModel(
        user: User.ToModel(),
        id: ID,
        start: BookingStart,
        end: BookingEnd,
        purpose: BookingPurpose,
        location: Location.ToModel(),
        inventories: ReservationInventories?.Select(x => x.Inventory.ToModel()).ToList()
        );
    }

    public void UpdateDb(ReservationModel model, AimasContext aimas)
    {
      if (model.BookingStart != default)
        BookingStart = model.BookingStart;
      if (model.BookingEnd != default)
        BookingEnd = model.BookingEnd;
      if (!string.IsNullOrEmpty(model.BookingPurpose))
        BookingPurpose = model.BookingPurpose;
      if (model.Location != null)
        Location = aimas.GetDbLocation(model.Location);


      // Update ReservationInventories
      var addAlerts = model.Inventories.Where(item => ReservationInventories.Find(item2 => item2.Inventory.ID == item.ID) == null).Select(item => item.CreateNewDbModel(aimas)).ToList();
      var removeAlerts = ReservationInventories.Where(item => model.Inventories.Find(item2 => item2.ID == item.Inventory.ID) == null).ToList();
      ReservationInventories.AddRange(addAlerts.Select(x => new ReservationInventoryModel_DB(this, x)));
      removeAlerts.ForEach(item => ReservationInventories.Remove(item));
    }
  }
}
