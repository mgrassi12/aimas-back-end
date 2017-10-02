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

    public ReportModel_DB ToDbModel()
    {
      return new ReportModel_DB(inventory: Inventory.ToDbModel(), id: ID, type: Type, creator: Creator.ToDbModel(), creationDate: CreationDate, executor: Executor.ToDbModel(), executionDate: ExecutionDate, notes: Notes);
    }
  }
}