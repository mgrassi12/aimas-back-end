using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AIMAS.Data.Identity;
using AIMAS.Data.Models;

namespace AIMAS.Data.Inventory
{
  [Table("reservation")]
  public class ReservationModel_DB : IAimasDbModel<ReservationModel>
  {
    [Key]
    public long ID { get; set; }

    [Key]
    public UserModel_DB User { get; set; }

    [Required, DataType(DataType.DateTime)]
    public DateTime BookingStart { get; set; }

    [Required, DataType(DataType.DateTime)]
    public DateTime BookingEnd { get; set; }

    [Required]
    public string BookingPurpose { get; set; }

    [Required]
    public LocationModel_DB Location { get; set; }

    public ReservationModel_DB() : base()
    {

    }

    public ReservationModel_DB(UserModel_DB user, DateTime start, DateTime end, string purpose, LocationModel_DB location, long id = default) : this()
    {
      ID = id;
      User = user;
      BookingStart = start;
      BookingEnd = end;
      BookingPurpose = purpose;
      Location = location;
    }

    public ReservationModel ToModel()
    {
      return new ReservationModel(id: ID, user: User.ToModel(), start: BookingStart, end: BookingEnd, purpose: BookingPurpose, location: Location.ToModel());
    }
  }
}
