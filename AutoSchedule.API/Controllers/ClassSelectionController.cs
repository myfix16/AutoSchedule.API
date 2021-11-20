using System.Collections.Generic;
using System.Linq;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AutoSchedule.API.Controllers
{
#if DEBUG
    [EnableCors(Startup.CorsAllowAllOrigins)]
#else
    [EnableCors(Startup.CorsAllowSpecificOrigins)]
#endif
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSelectionController : ControllerBase
    {
        // POST api/<ClassSelectionController>/5
        [HttpPost("{maxSchedules:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Schedule>> MakeSchedules([FromBody, BindRequired] IEnumerable<PriorityClass> classesToSelect, int maxSchedules = -1)
        {
            // parse arguments
            if (maxSchedules == -1) maxSchedules = int.MaxValue;

            List<Course> courses = classesToSelect
                .Select(course => new Course(
                    course.Name,
                    Program.GroupedSessions.First(g => g.Key == course.Name),
                    course.Priority))
                .ToList();

            // higher priority value => lower priority, therefore, the sequence is required-preferred-optional
            PriorityQueue<Schedule, Schedule.PriorityValue> schedules = ClassSelector.FindSchedules(
                courses.OrderBy(c => c.Priority), maxSchedules);

            if (schedules.Count == 0) return NoContent();

            List<Schedule> result = new(schedules.Count);
            do
            {
                result.Add(schedules.Dequeue());
            } while (schedules.Count != 0);

            return result;
        }
    }
}
