using System;
using System.Collections.Generic;
using System.Linq;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Priority_Queue;

namespace AutoSchedule.Core.Test
{
    [TestClass]
    public class UnitTestClassSelection
    {
        static readonly List<Session> Sessions = new()
        {
            new Session("LEC", "Subject1 L01", "1000", "Staff", "Cheng Dao Bldg 101", new List<SessionTime>()
            {
                new(DayOfWeek.Monday, new Time(8, 30), new Time(9,50)),
                new(DayOfWeek.Wednesday, new Time(8, 30), new Time(9,50)),
            }),
            new Session("LEC", "Subject2 L01", "1001", "Staff", "Zhi Xin Bldg 103", new List<SessionTime>()
            {
                new(DayOfWeek.Tuesday, new Time(8, 30), new Time(9,50)),
                new(DayOfWeek.Thursday, new Time(8, 30), new Time(9,50)),
            }),
            new Session("LEC", "Subject3 L01", "1002", "Staff", "Teaching A 102", new List<SessionTime>()
            {
                new(DayOfWeek.Monday, new Time(10, 00), new Time(11,50)),
                new(DayOfWeek.Wednesday, new Time(10, 00), new Time(11,50)),
            }),
            new Session("LEC", "Subject3 L02", "1003", "Staff", "Teaching A 102", new List<SessionTime>()
            {
                new(DayOfWeek.Monday, new Time(9, 55), new Time(11,50)),
                new(DayOfWeek.Wednesday, new Time(10, 00), new Time(11,50)),
            }),
            new Session("LEC", "Subject3 L03", "1004", "Staff", "Teaching A 102", new List<SessionTime>()
            {
                new(DayOfWeek.Monday, new Time(13, 30), new Time(15,50)),
                new(DayOfWeek.Wednesday, new Time(13, 30), new Time(15,50)),
            }),
        };

        static readonly List<IGrouping<string, Session>> GroupedSessions =
            Sessions.GroupBy(s => s.GetClassifiedName()).ToList();

        [TestMethod]
        public void TestClassNumPriority()
        {
            // todo: fill class number priority test
            List<PriorityClass> classesToSelect = new()
            {
                new PriorityClass("Subject1 LEC", Priority.Required),
                new PriorityClass("Subject2 LEC", Priority.Preferred),
                new PriorityClass("Subject3 LEC", Priority.Optional),
            };

            SimplePriorityQueue<Schedule, Schedule.PriorityValue> schedules = GenerateSchedules(classesToSelect);

            Console.WriteLine("Finished");
        }

        [TestMethod]
        public void TestLocationPriority()
        {
            List<PriorityClass> classesToSelect = new()
            {
                new PriorityClass("Subject1 LEC", Priority.Required),
                new PriorityClass("Subject2 LEC", Priority.Preferred),
                new PriorityClass("Subject3 LEC", Priority.Optional),
            };

            SimplePriorityQueue<Schedule, Schedule.PriorityValue> schedules = GenerateSchedules(classesToSelect);
            List<Schedule> result = new(schedules.Count);
            while (schedules.Count != 0)
            {
                result.Add(schedules.Dequeue());
            }
            CollectionAssert.AreEqual(result.OrderByDescending(s => s.Priority.LocationPriority).ToList(), result);
        }

        private static SimplePriorityQueue<Schedule, Schedule.PriorityValue> GenerateSchedules(
            IEnumerable<PriorityClass> classesToSelect, int maxSchedules = 10)
        {
            List<Course> courses = classesToSelect
                .Select(course => new Course(
                    course.Name,
                    GroupedSessions.First(g => g.Key == course.Name),
                    course.Priority))
                .ToList();

            SimplePriorityQueue<Schedule, Schedule.PriorityValue> schedules = ClassSelector.FindSchedules(
                courses.OrderByDescending(c => c.Priority), maxSchedules);

            return schedules;
        }
    }
}
