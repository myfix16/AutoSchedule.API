using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace AutoSchedule.Core.Models
{
    public static class ClassSelector
    {
        /// <summary>
        /// Find schedules based on given courses.
        /// </summary>
        /// <param name="allCourses">A list of courses that need to be enrolled, ordered descending by priority.</param>
        /// <param name="maxSchedules">Specify the maximum number of schedule to provide.</param>
        /// <returns>Possible schedules</returns>
        public static IPriorityQueue<Schedule, Schedule.PriorityValue> FindSchedules(IEnumerable<Course> allCourses, int maxSchedules = 10)
        {
            int id = 0;
            var outcome = new SimplePriorityQueue<Schedule, Schedule.PriorityValue>(); // A min-heap

            // todo: add support to course priority
            // Inner function that finds all suitable schedules. The core idea is Eight Queen, modified with priority
            void Enroll(IEnumerable<Course> courses, Schedule currentSchedule, int _maxSchedules)
            {
                if (id >= _maxSchedules) return;

                if (courses.Any())
                {
                    Course currentCourse = courses.First();
                    var validSessions = currentCourse.Where(currentSchedule.Validate);
                    // if this course can be added, add it normally
                    if (validSessions.Any())
                    {
                        foreach (var session in validSessions)
                        {
                            Enroll(courses.Skip(1), currentSchedule.WithAdded(session, currentCourse.Priority), _maxSchedules);
                        }
                    }
                    // if this course cannot be added but this course is not required, skip it and continue
                    else if (currentCourse.Priority != Priority.Required)
                    {
                        Enroll(courses.Skip(1), currentSchedule, _maxSchedules);
                    }
                }
                else
                {
                    outcome.Enqueue(currentSchedule, currentSchedule.Priority);
                    currentSchedule.Id = (++id).ToString();
                }
            }

            Enroll(allCourses, new Schedule { Id = id.ToString() }, maxSchedules);

            return outcome;
        }
    }
}
