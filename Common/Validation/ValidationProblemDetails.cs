using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

/* Ausfuehrliche Validierungsfehler zurueckgeben
 * -> https://www.strathweb.com/2018/07/centralized-exception-handling-and-request-validation-in-asp-net-core/
 * 
 * Ergebnis sieht so aus:
 * { "title": "Request Validation Errors",
 *   "ValidationErrors": [
 *     { "Name": "id", "Description": "The value 'test' is not valid." },  // "id" ist der Feldname
 *     { ... }
 *   ]
 * }
 */
namespace hb.Common.Validation {

  public class ValidationProblemDetails : ProblemDetails {
    public ICollection<ValidationError> ValidationErrors { get; set; }
  }

  public class ValidationError {
    public string Name { get; set; }
    public string Description { get; set; }
  }

  public class ValidationProblemDetailsResult : IActionResult {
    public Task ExecuteResultAsync(ActionContext context) {
      var modelStateEntries = context.ModelState.Where(e => e.Value.Errors.Count > 0).ToArray();
      var errors = new List<ValidationError>();

      if (modelStateEntries.Any()) {
        foreach (var modelStateEntry in modelStateEntries) {
          foreach (var modelStateError in modelStateEntry.Value.Errors) {
            var error = new ValidationError {
              Name = modelStateEntry.Key,
              Description = modelStateError.ErrorMessage
            };
            errors.Add(error);
          }
        }
      }

      var problemDetails = new ValidationProblemDetails {
        Title = "Request Validation Error",
        ValidationErrors = errors
      };
      context.HttpContext.Response.StatusCode = 422; // unprocessable entity
      var json = JsonConvert.SerializeObject(problemDetails);
      context.HttpContext.Response.WriteAsync(json);
      return Task.CompletedTask;
    }
  }
}
