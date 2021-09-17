using System.Collections.Generic;
using System.Threading.Tasks;
using AutoSchedule.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoSchedule.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        // GET: api/<SessionsController>
        [HttpGet]
        public async Task<IEnumerable<Session>> GetSessions()
        {
            Program.Sessions ??= await Program.DataProvider.GetSessionsAsync();
            return Program.Sessions;
        }
    }
}
