using System.Collections.Generic;
using System.Linq;
using AutoSchedule.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Priority_Queue;

namespace AutoSchedule.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSelectionController : ControllerBase
    {
        // POST api/<ClassSelectionController>/5
        [HttpPost("{maxSchedules:int}")]
        public IEnumerable<Schedule> MakeSchedules(int maxSchedules, [FromBody] IEnumerable<PriorityClass> classesToSelect)
        {
            List<Course> courses = classesToSelect
                .Select(course => new Course(
                    course.Name,
                    Program.GroupedSessions.First(g => g.Key == course.Name),
                    course.Priority))
                .ToList();

            IPriorityQueue<Schedule, Schedule.PriorityValue> schedules = ClassSelector.FindSchedules(
                courses.OrderByDescending(c => c.Priority), maxSchedules);

            return schedules.ToList();
        }
    }
}
