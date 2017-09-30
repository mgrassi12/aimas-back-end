using System;
using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class ReservationModel : IAimasModel<ReservationModel_DB>
  {
    public long ID { get; set; }
    public UserModel User { get; set; }
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnd { get; set; }
    public string BookingPurpose { get; set; }
    public LocationModel Location { get; set; }

    public ReservationModel()
    {

    }

    public ReservationModel(UserModel user, DateTime start, DateTime end, string purpose, LocationModel location, long id = default) : this()
    {
      ID = id;
      User = user;
      BookingStart = start;
      BookingEnd = end;
      BookingPurpose = purpose;
      Location = location;
    }

    public ReservationModel_DB ToDbModel()
    {
      return new ReservationModel_DB(id: ID, user: User.ToDbModel(), start: BookingStart, end: BookingEnd, purpose: BookingPurpose, location: Location.ToDbModel());
    }
  }
}
