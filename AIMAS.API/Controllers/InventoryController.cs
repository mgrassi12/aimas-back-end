using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;

namespace AIMAS.API.Controllers
{
  [Route("api/inventory")]
  public class InventoryController : Controller
  {
    private ILogger log;

    private InventoryDB Inventory { get; }

    public InventoryController(InventoryDB inventoryDB)
    {
      log = Startup.LoggerFactory.CreateLogger<InventoryController>();
      Inventory = inventoryDB;
    }

    [HttpGet]
    [Route("all")]
    [Authorize(Roles = "User")]
    public Result<List<InventoryModel>> GetInventory()
    {
      var result = new Result<List<InventoryModel>>();

      try
      {
        var items = Inventory.GetInventories();
        result.Success = true;
        result.ReturnObj = items;

      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }
  }
}
