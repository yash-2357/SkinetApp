using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {

  public class ErrorController : BaseAPIController { 
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorised() {
      return Unauthorized();
    }

    [HttpGet("badrequest")]
    public IActionResult GetBadRequest() {
      return BadRequest("It is bad request");
    }

    [HttpGet("notfound")]
    public IActionResult GetNotFound() {
      return NotFound();
    }

    [HttpGet("internalerror")]
    public IActionResult GetInternalError() {
      throw new Exception("internal server error");
    }

    [HttpPost("Validationerror")]
    public IActionResult GetValidationError(Product product) {
      return Ok();
    }
  }
}
