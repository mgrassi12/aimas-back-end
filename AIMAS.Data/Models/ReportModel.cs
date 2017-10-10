using AIMAS.Data.Inventory;
using System;

namespace AIMAS.Data.Models
{
  public class ReportModel : IAimasModel<ReportModel_DB>
  {
    public InventoryModel Inventory { get; set; }
    public long ID { get; set; }
    public string Type { get; set; }
    public UserModel Creator { get; set; }
    public DateTime CreationDate { get; set; }
    public UserModel Executor { get; set; }
    public DateTime ExecutionDate { get; set; }
    public string Notes { get; set; }

    public ReportModel(InventoryModel inventory, string type, UserModel creator, DateTime creationDate, UserModel executor, DateTime executionDate, string notes, long id = default)
    {
      Inventory = inventory;
      ID = id;
      Type = type;
      Creator = creator;
      CreationDate = creationDate;
      Executor = executor;
      ExecutionDate = executionDate;
      Notes = notes;
    }

    public ReportModel_DB CreateNewDbModel(AimasContext aimas)
    {
      var dbInventory = aimas.GetDbInventory(Inventory);
      var dbCreator = aimas.GetDbUser(Creator);
      var dbExecutor = aimas.GetDbUser(Executor);
      return new ReportModel_DB(inventory: dbInventory, id: ID, type: Type, creator: dbCreator, creationDate: CreationDate, executor: dbExecutor, executionDate: ExecutionDate, notes: Notes);
    }
  }
}
