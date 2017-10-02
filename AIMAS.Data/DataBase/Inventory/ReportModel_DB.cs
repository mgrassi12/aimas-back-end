using AIMAS.Data.Identity;
using AIMAS.Data.Models;
using AIMAS.Data.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIMAS.Data.Inventory
{
  [Table("report")]
  public class ReportModel_DB : IAimasDbModel<ReportModel>
  {
    [Required]
    public InventoryModel_DB Inventory { get; set; }

    [Key]
    public long ID { get; set; }

    [Required]
    public string Type { get; set; }

    [Required]
    public UserModel_DB Creator { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime CreationDate { get; set; }

    [Required]
    public UserModel_DB Executor { get; set; }

    [Required, Column(TypeName = "timestamptz"), DateTimeKind(DateTimeKind.Utc)]
    public DateTime ExecutionDate { get; set; }

    public string Notes { get; set; }

    public ReportModel_DB(InventoryModel_DB inventory, string type, UserModel_DB creator, DateTime creationDate, UserModel_DB executor, DateTime executionDate, string notes, long id = default)
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

    public ReportModel ToModel()
    {
      return new ReportModel(inventory: Inventory.ToModel(), id: ID, type: Type, creator: Creator.ToModel(), creationDate: CreationDate, executor: Executor.ToModel(), executionDate: ExecutionDate, notes: Notes);
    }
  }
}
