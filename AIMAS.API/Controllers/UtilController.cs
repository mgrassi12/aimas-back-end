using System;
using System.Collections.Generic;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AIMAS.Data.Util;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AIMAS.API.Controllers
{
  [Route("api/util")]
  public class UtilController : Controller
  {
    public InventoryDB InventoryDb { get; }

    public UtilController(InventoryDB inventoryDb)
    {
      InventoryDb = inventoryDb;
    }

    [HttpPost]
    [Route("add")]
    [Authorize(Roles = Roles.Admin)]
    public Result AddLocation([FromBody]LocationModel location)
    {
      var result = new Result();

      try
      {
        InventoryDb.AddLocation(location);
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
        result.ReturnObj = InventoryDb.GetLocations();
        result.Success = true;
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
    public PageResultObj<List<LocationModel>> GetLocations([FromBody]LocationSearch search)
    {
      var result = new PageResultObj<List<LocationModel>>();

      try
      {
        var items = InventoryDb.GetLocations(search);
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
    [Route("update")]
    [Authorize(Roles = Roles.Admin)]
    public Result UpdateLocation([FromBody]LocationModel location)
    {
      var result = new Result();

      try
      {
        InventoryDb.UpdateLocation(location);
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
    public Result RemoveLocation(long id)
    {
      var result = new Result();

      try
      {
        InventoryDb.RemoveLocation(id);
        result.Success = true;
      }
      catch (Exception ex)
      {
        result.AddException(ex);
      }

      return result;
    }



        [HttpGet]
        [Route("export")]
        [Authorize]
        public ResultObj<string> GetExport()
        {
            var result = new ResultObj<string>();

            try
            {
                result.ReturnObj = DataExporter.CreateCSV(InventoryDb.GetInventories());
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
