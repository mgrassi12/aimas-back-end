using System;
using System.Collections.Generic;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AIMAS.Data.Util;
using AIMAS.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AIMAS.API.Controllers
{
  [Route("api/util")]
  public class UtilController : Controller
  {
    public InventoryDB InventoryDb { get; }
    public AimasContext Aimas { get; }

    public UtilController(InventoryDB inventoryDb, AimasContext aimas)
    {
      InventoryDb = inventoryDb;
      Aimas = aimas;
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

    [HttpGet]
    [Route("export")]
    [Authorize]
    public ResultObj<string> GetExport()
    {
      var result = new ResultObj<string>();

      try
      {
        var allData = Aimas.Inventories
          .Include(x => x.CurrentLocation)
          .Include(x => x.DefaultLocation)
          .ToList();
        result.ReturnObj = DataExporter.CreateCSV(allData);
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
