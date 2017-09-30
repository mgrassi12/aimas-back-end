namespace AIMAS.Data.Models
{
  public interface IAimasModel<DbModel>
    {
    DbModel ToDbModel();
    }
}
