using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)] // this helps avoid issues with swagger
    public class ErrorController : BaseApiController
    {
        // I tried using this controller method inside BuggyController instead of creating this 
        // new controller. It did not work.
        // Route override needs to be applied to the Class.
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }    }
}