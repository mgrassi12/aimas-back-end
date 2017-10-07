using AIMAS.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{

  [Table("alertTime")]
  public class AlertTimeModel_DB : IAimasDbModel<AlertTimeModel>
  {

    [Key]
    public long ID { get; set; }

    [Required]
    public long DaysBefore { get; set; }

    public string Name { get; set; }

    private AlertTimeModel_DB()
    {

    }

    public AlertTimeModel_DB(long daysBefore, string name = default, long id = default)
    {
      ID = id;
      DaysBefore = daysBefore;
      Name = name;
    }

    public AlertTimeModel ToModel()
    {
      return new AlertTimeModel(id: ID, name: Name, daysBefore: DaysBefore);
    }

    public void UpdateDb(AlertTimeModel alertTime, AimasContext aimas)
    {
      Name = alertTime.Name;

      if (alertTime.DaysBefore != default)
        DaysBefore = alertTime.DaysBefore;
    }
  }
}
