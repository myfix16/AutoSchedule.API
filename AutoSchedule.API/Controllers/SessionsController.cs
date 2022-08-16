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
        /// <summary>
        /// Get all available sessions in Json format.
        /// </summary>
        /// <returns>A list of sessions</returns>
        [HttpGet]
        public IEnumerable<Session> GetSessions([FromQuery(Name = "term")] string term)
        {
            if (!Program.Sessions.ContainsKey(term)) Program.PrepareData(term);
            return Program.Sessions[term];
        }

        // GET: api/<SessionsController>/ClassNames
        [HttpGet("ClassNames")]
        public IEnumerable<string> GetClassNames([FromQuery(Name = "term")] string term)
        {
            if (!Program.ClassNames.ContainsKey(term)) Program.PrepareData(term);
            return Program.ClassNames[term];
        }
    }
}
