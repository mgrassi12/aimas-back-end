using System;
using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class ReservationModel : IAimasModel<ReservationModel_DB>
  {
    public UserModel User { get; set; }
    public long ID { get; set; }
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnd { get; set; }
    public string BookingPurpose { get; set; }
    public LocationModel Location { get; set; }

    private ReservationModel()
    {

    }

    public ReservationModel(UserModel user, DateTime start, DateTime end, string purpose, LocationModel location, long id = default)
    {
      ID = id;
      User = user;
      BookingStart = start;
      BookingEnd = end;
      BookingPurpose = purpose;
      Location = location;
    }

    public ReservationModel_DB CreateNewDbModel(AimasContext aimas)
    {
      var dbUser = aimas.GetDbUser(User);
      var dbLocation = aimas.GetDbLocation(Location);
      return new ReservationModel_DB(user: dbUser, id: ID, start: BookingStart, end: BookingEnd, purpose: BookingPurpose, location: dbLocation);
    }
  }
}
