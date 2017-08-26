using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace AIMAS.Data.Models
{
  public class Result
  {
    public bool Success { get; set; }
    public List<Error> Errors { get; set; }


    public void AddException(Exception ex)
    {
      Errors.Add(new Error() { Code = "101", Description = ex.Message, StackTrace = ex.StackTrace });
      if (ex.InnerException != null && ex != ex.InnerException)
        AddException(ex.InnerException);
    }

    public void AddIdentityErrors(IEnumerable<IdentityError> identityErrors)
    {
      Errors = identityErrors.Select(error => new Error() { Code = error.Code, Description = error.Description }).ToList();
    }
  }

  public class Result<TReturnType> : Result
  {
    public TReturnType ReturnObj { get; set; }
  }

  public class Error
  {
    public string Code { get; set; }
    public string Description { get; set; }
    public string StackTrace { get; set; }
  }
}
