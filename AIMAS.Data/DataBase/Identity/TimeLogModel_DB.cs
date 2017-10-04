using AIMAS.Data.Models;
using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Identity
{
  [Table("timeLog")]
  public class TimeLogModel_DB : IAimasDbModel<TimeLogModel>
  {
    [Required]
    public UserModel_DB User { get; set; }

    [Key]
    public long ID { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime CheckIn { get; set; }

    [Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime CheckOut { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime CheckInLodged { get; set; }

    [Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime CheckOutLodged { get; set; }

    [Required]
    public string Purpose { get; set; }

    private TimeLogModel_DB()
    {
    }

    public TimeLogModel_DB(UserModel_DB user, DateTime checkIn, DateTime checkInLodged, string purpose, DateTime checkOut = default, DateTime checkOutLodged = default, long id = default)
    {
      ID = id;
      User = user;
      CheckIn = checkIn;
      CheckOut = checkOut;
      CheckInLodged = checkInLodged;
      CheckOutLodged = checkOutLodged;
      Purpose = purpose;
    }

    public TimeLogModel ToModel()
    {
      return new TimeLogModel(id: ID, user: User.ToModel(), checkIn: CheckIn, checkOut: CheckOut, checkInLodged: CheckInLodged, checkOutLodged: CheckOutLodged, purpose: Purpose);
    }
  }
}
