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
  public class ReservationModel_DB : IAimasDbModel<ReservationModel>
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

    [Required]
    public bool IsArchived { get; set; }

    private ReservationModel_DB()
    {
    }

    public ReservationModel_DB(UserModel_DB user, DateTime start, DateTime end, string purpose, LocationModel_DB location, long id = default)
    {
      ID = id;
      User = user;
      BookingStart = start;
      BookingEnd = end;
      BookingPurpose = purpose;
      Location = location;
      ReservationInventories = new List<ReservationInventoryModel_DB>();
    }

    public ReservationModel ToModel()
    {
      return new ReservationModel(user: User.ToModel(), id: ID, start: BookingStart, end: BookingEnd, purpose: BookingPurpose, location: Location.ToModel());
    }
  }
}
