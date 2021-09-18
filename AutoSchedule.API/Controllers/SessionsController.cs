using System.Collections.Generic;
using AutoSchedule.Core.Models;
using Microsoft.AspNetCore.Cors;
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
    public class SessionsController : ControllerBase
    {
        // GET: api/<SessionsController>
        [HttpGet]
        public IEnumerable<Session> GetSessions() => Program.Sessions;

        // GET: api/<SessionsController>/ClassNames
        [HttpGet("ClassNames")]
        public IEnumerable<string> GetClassNames() => Program.ClassNames;
    }
}
