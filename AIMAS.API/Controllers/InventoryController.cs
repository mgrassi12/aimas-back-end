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
using AIMAS.Data.Identity;

namespace AIMAS.API.Controllers
{
  [Route("api/inventory")]
  public class InventoryController : Controller
  {
    private ILogger log;

    private InventoryDB InventoryDB { get; }

    public InventoryController(InventoryDB inventoryDB)
    {
      log = Startup.LoggerFactory.CreateLogger<InventoryController>();
      InventoryDB = inventoryDB;
    }

    [HttpGet]
    [Route("all")]
    [Authorize]
    public ResultObj<List<InventoryModel>> GetInventory()
    {
      var result = new ResultObj<List<InventoryModel>>();

      try
      {
        var items = InventoryDB.GetInventories();
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
    [Authorize]
    public PageResultObj<List<InventoryModel>> GetInventory([FromBody]InventorySearch search)
    {
      var result = new PageResultObj<List<InventoryModel>>();

      try
      {
        var items = InventoryDB.GetInventories(search);
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
    [Authorize(Roles = Roles.Admin)]
    public Result AddInventory([FromBody]InventoryModel inventory)
    {
      var result = new Result();

      try
      {
        InventoryDB.AddInventory(inventory);
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
    [Authorize(Roles = Roles.Admin)]
    public Result UpdateInventory([FromBody]InventoryModel inventory)
    {
      var result = new Result();

      try
      {
        InventoryDB.UpdateInventory(inventory);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }

    [HttpGet]
    [Route("remove/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public Result RemoveInventory(long id)
    {
      var result = new Result();

      try
      {
        InventoryDB.RemoveInventory(id);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }

    [HttpGet]
    [Route("locations")]
    [Authorize]
    public ResultObj<List<LocationModel>> GetLocations()
    {
      var result = new ResultObj<List<LocationModel>>();

      try
      {
        result.ReturnObj = InventoryDB.GetLocations();
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }

    [HttpGet]
    [Route("alerts/{id}")]
    [Authorize]
    public ResultObj<List<InventoryAlertTimeModel>> GetInventoryAlertTimes(long id)
    {
      var result = new ResultObj<List<InventoryAlertTimeModel>>();

      try
      {
        result.ReturnObj = InventoryDB.GetInventoryAlertTimes(id);
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
