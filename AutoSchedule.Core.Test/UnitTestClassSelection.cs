using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            new Session("LEC", "Subject4 L01", "1005", "Staff", "Teaching A 102", new List<SessionTime>()
            {
                new(DayOfWeek.Monday, new Time(10, 30), new Time(15,50)),
                new(DayOfWeek.Wednesday, new Time(10, 30), new Time(15,50)),
            }),
        };

        static readonly List<IGrouping<string, Session>> GroupedSessions =
            Sessions.GroupBy(s => s.GetClassifiedName()).ToList();

        [TestMethod]
        public void TestClassNumPriority()
        {
            List<PriorityClass> classesToSelect = new()
            {
                new PriorityClass("Subject1 LEC", Priority.Required),
                new PriorityClass("Subject2 LEC", Priority.Preferred),
                new PriorityClass("Subject4 LEC", Priority.Preferred),
                new PriorityClass("Subject3 LEC", Priority.Optional),
            };

            List<Schedule> result = GenerateSchedules(classesToSelect);

            IFormatter formatter = new BinaryFormatter();
            var path = $"{ProjectSourcePath.Value}\\TestClassNumPriorityRef.bin";
            // // Serialize the correct result to a file
            // using (FileStream outputFileStream = File.OpenWrite(path))
            // {
            //     formatter.Serialize(outputFileStream, result);
            // }

            // De-serialize previously checked result
            List<Schedule> reference;
            using (FileStream inputFileStream = File.OpenRead(path))
            {
                reference = formatter.Deserialize(inputFileStream) as List<Schedule>;
            }

            Assert.IsTrue(reference.SequenceEqual(result));
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

            List<Schedule> result = GenerateSchedules(classesToSelect);

            IFormatter formatter = new BinaryFormatter();
            var path = $"{ProjectSourcePath.Value}\\TestLocationPriorityRef.bin";
            // // Serialize the correct result to a file
            // using (FileStream outputFileStream = File.OpenWrite(path))
            // {
            //     formatter.Serialize(outputFileStream, result);
            // }

            // De-serialize previously checked result
            List<Schedule> reference;
            using (FileStream inputFileStream = File.OpenRead(path))
            {
                reference = formatter.Deserialize(inputFileStream) as List<Schedule>;
            }

            Assert.IsTrue(reference.SequenceEqual(result));
        }

        static List<Schedule> GenerateSchedules(IEnumerable<PriorityClass> classesToSelect, int maxSchedules = 10)
        {
            List<Course> courses = classesToSelect
                .Select(course => new Course(
                    course.Name,
                    GroupedSessions.First(g => g.Key == course.Name),
                    course.Priority))
                .ToList();

            // higher priority value => lower priority, therefore, the sequence is required-preferred-optional
            PriorityQueue<Schedule, Schedule.PriorityValue> schedules = ClassSelector.FindSchedules(
                courses.OrderBy(c => c.Priority), maxSchedules);

            List<Schedule> result = new(schedules.Count);
            while (schedules.Count != 0)
            {
                result.Add(schedules.Dequeue());
            }

            return result;
        }
    }
}
