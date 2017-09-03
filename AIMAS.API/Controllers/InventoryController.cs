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
    public ResultObj<List<InventoryModel>> GetInventory()
    {
      var result = new ResultObj<List<InventoryModel>>();

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

    [HttpPost]
    [Route("search")]
    [Authorize(Roles = "User")]
    public PageResultObj<List<InventoryModel>> GetInventory([FromBody]InventorySearch search)
    {
      var result = new PageResultObj<List<InventoryModel>>();

      try
      {
        var items = Inventory.GetInventories(search);
        result.Success = true;
        result.ReturnObj = items.list;
        result.TotalCount = items.TotalCount;
        result.PageIndex = search.PageIndex;
        result.PageSize = search.PageSize;

      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }

    [HttpPost]
    [Route("add")]
    [Authorize(Roles = "Admin")]
    public Result AddInventory([FromBody]InventoryModel inventory)
    {
      var result = new Result();

      try
      {
        Inventory.AddInventory(inventory);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }

    [HttpPost]
    [Route("update")]
    [Authorize(Roles = "Admin")]
    public Result UpdateInventory([FromBody]InventoryModel inventory)
    {
      var result = new Result();

      try
      {
        Inventory.UpdateInventory(inventory);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }
  }
}
