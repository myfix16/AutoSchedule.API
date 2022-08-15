using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoSchedule.API.Controllers
{
#if DEBUG
    [EnableCors(Startup.CorsAllowAllOrigins)]
#else
    [EnableCors(Startup.CorsAllowSpecificOrigins)]
#endif
    [Route("api/[controller]")]
    [ApiController]
    public class TermsController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IEnumerable<string> GetAvailableTerms() => Program.Terms;
    }
}
