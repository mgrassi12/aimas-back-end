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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AIMAS.API.Controllers
{
  [Route("api/reservation")]
  public class ReservationController : Controller
  {
    public InventoryDB InventoryDB { get; }

    public ReservationController(InventoryDB inventoryDB)
    {
      InventoryDB = inventoryDB;
    }


    [HttpPost]
    [Route("search")]
    [Authorize]
    public PageResultObj<List<ReservationModel>> GetReservation([FromBody]ReservationSearch search)
    {
      var result = new PageResultObj<List<ReservationModel>>();

      try
      {
        var items = InventoryDB.GetReservations(search);
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
    public Result AddReservation([FromBody]ReservationModel reservation)
    {
      var result = new Result();

      try
      {
        InventoryDB.Addreservation(reservation);
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
    public Result UpdateReservation([FromBody]ReservationModel reservation)
    {
      var result = new Result();

      try
      {
        InventoryDB.UpdateReservation(reservation);
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
    public Result RemoveReservation(long id)
    {
      var result = new Result();

      try
      {
        InventoryDB.RemoveReservation(id);
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
