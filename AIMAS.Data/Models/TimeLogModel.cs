using AIMAS.Data.Identity;
using System;

namespace AIMAS.Data.Models
{
  public class TimeLogModel : IAimasModel<TimeLogModel_DB>
  {
    public UserModel User { get; set; }
    public long ID { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public DateTime CheckInLodged { get; set; }
    public DateTime CheckOutLodged { get; set; }
    public string Purpose { get; set; }

    public TimeLogModel(UserModel user, DateTime checkIn, DateTime checkInLodged, string purpose, DateTime checkOut = default, DateTime checkOutLodged = default, long id = default)
    {
      ID = id;
      User = user;
      CheckIn = checkIn;
      CheckOut = checkOut;
      CheckInLodged = checkInLodged;
      CheckOutLodged = checkOutLodged;
      Purpose = purpose;
    }

    public TimeLogModel_DB CreateNewDbModel(AimasContext aimas)
    {
      var dbUser = aimas.GetDbUser(User);
      return new TimeLogModel_DB(id: ID, user: dbUser, checkIn: CheckIn, checkOut: CheckOut, checkInLodged: CheckInLodged, checkOutLodged: CheckOutLodged, purpose: Purpose);
    }
  }
}
