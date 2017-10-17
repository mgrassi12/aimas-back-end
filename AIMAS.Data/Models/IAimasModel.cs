namespace AIMAS.Data.Models
{
  public interface IAimasModel<DbModel>
    {
    DbModel CreateNewDbModel(AimasContext aimas);
    }
}
