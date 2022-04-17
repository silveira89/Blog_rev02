using Blog_rev02.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog_rev02.Controllers {

    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase {

        [HttpGet("")]
        [ApiKey]
        public IActionResult Get() {
            return Ok();
        }
    }
}
