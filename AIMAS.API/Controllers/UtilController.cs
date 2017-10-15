using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AIMAS.API.Controllers
{
  [Route("api/util")]
  public class UtilController : Controller
  {
    public InventoryDB InventoryDB { get; }

    public UtilController(InventoryDB inventoryDB)
    {
      InventoryDB = inventoryDB;
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
  }
}
