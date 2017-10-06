using AIMAS.Data.Inventory;

namespace AIMAS.Data.Models
{
  public class AlertTimeModel : IAimasModel<AlertTimeModel_DB>
  {
    public long ID { get; set; }
    public long DaysBefore { get; set; }
    public string Name { get; set; }

    public AlertTimeModel()
    {

    }

    public AlertTimeModel(long daysBefore, string name = default, long id = default)
    {
      ID = id;
      DaysBefore = daysBefore;
      Name = name;
    }

    public AlertTimeModel_DB ToDbModel()
    {
      return new AlertTimeModel_DB(id: ID, name: Name, daysBefore: DaysBefore);
    }
  }
}
