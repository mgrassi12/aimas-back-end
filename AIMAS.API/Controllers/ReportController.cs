using System;
using System.Collections.Generic;
using AIMAS.Data.Identity;
using AIMAS.Data.Inventory;
using AIMAS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIMAS.API.Controllers
{
  [Route("api/report")]
  public class ReportController : Controller
  {
    private InventoryDB InventoryDb { get; }

    public ReportController(InventoryDB inventoryDb)
    {
      InventoryDb = inventoryDb;
    }

    [HttpGet]
    [Route("all")]
    [Authorize]
    public ResultObj<List<ReportModel>> GetReports()
    {
      var result = new ResultObj<List<ReportModel>>();

      try
      {
        var items = InventoryDb.GetReports();
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
    public PageResultObj<List<ReportModel>> GetReports([FromBody]ReportSearch search)
    {
      var result = new PageResultObj<List<ReportModel>>();

      try
      {
        var items = InventoryDb.GetReports(search);
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
    [Authorize]
    public Result AddReport([FromBody]ReportModel report)
    {
      var result = new Result();

      try
      {
        InventoryDb.AddReport(report);
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
    public Result RemoveReport(long id)
    {
      var result = new Result();

      try
      {
        InventoryDb.RemoveReport(id);
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
