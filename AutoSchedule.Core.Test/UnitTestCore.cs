using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoSchedule.Core.Models;
using Priority_Queue;

namespace AutoSchedule.Core.Test
{
    [TestClass]
    public class UnitTestCore
    {
        [TestMethod]
        public void TestClassSelector()
        {
            #region test data
            List<List<Session>> allLectures = new List<List<Session>>
            {
                // FIN 3080
                new List<Session>
                {
                    new Session
                        ("Lecture","FIN3080","1369","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Thursday,new Time(8,30),new Time(10,20)),
                            new SessionTime(DayOfWeek.Friday,new Time(8,30),new Time(9,20)),
                        }),
                    new Session
                        ("Lecture","FIN3080","1370","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Thursday,new Time(10,30),new Time(12,20)),
                            new SessionTime(DayOfWeek.Friday,new Time(10,30),new Time(11,20)),
                        }),
                    new Session
                        ("Lecture","FIN3080","1371","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Thursday,new Time(13,30),new Time(15,20)),
                            new SessionTime(DayOfWeek.Friday,new Time(13,30),new Time(14,20)),
                        }),
                },
                // FIN 4110
                new List<Session>
                {
                    new Session
                        ("Lecture","FIN4110","1384","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Wednesday,new Time(10,30),new Time(11,50)),
                            new SessionTime(DayOfWeek.Friday,new Time(9,00),new Time(10,20)),
                        }),
                    new Session
                        ("Lecture","FIN4110","1385","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Wednesday,new Time(13,30),new Time(14,50)),
                            new SessionTime(DayOfWeek.Friday,new Time(10,30),new Time(11,50)),
                        }),
                },
                // FIN 4210
                new List<Session>
                {
                    new Session
                        ("Lecture","FIN4210","1778","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(10,30),new Time(11,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(10,30),new Time(11,50)),
                        }),
                },
                // MAT 3007
                new List<Session>
                {
                    new Session
                        ("Lecture","MAT3007","1445","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Wednesday,new Time(14,00),new Time(15,20)),
                            new SessionTime(DayOfWeek.Friday,new Time(14,00),new Time(15,20)),
                        }),
                },
                // MAT 2002
                new List<Session>
                {
                    new Session
                        ("Lecture","MAT2002","1426","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(10,30),new Time(11,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(10,30),new Time(11,50)),
                        }),
                    new Session
                        ("Lecture","MAT2002","1427","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(13,30),new Time(14,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(13,30),new Time(14,50)),
                        }),
                },
                // FTE 4560
                new List<Session>
                {
                    new Session
                        ("Lecture","FTE4560","2062","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(8,30),new Time(9,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(8,30),new Time(9,50)),
                        }),
                },
                // DMS 3003
                new List<Session>
                {
                    new Session
                        ("Lecture","DMS3003","1936","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(8,30),new Time(9,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(8,30),new Time(9,50)),
                        }),
                    new Session
                        ("Lecture","DMS3003","1937","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(10,30),new Time(11,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(10,30),new Time(11,50)),
                        }),
                },
                // GEB 2404
                new List<Session>
                {
                    new Session
                        ("Lecture","GEB2404","2111","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Friday,new Time(8,30),new Time(10,20)),
                        }),
                },
                // GEB 2404 TUT
                new List<Session>
                {
                    new Session
                        ("Lecture","GEB2404T","2112","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Wednesday,new Time(8,30),new Time(10,20)),
                        }),
                    new Session
                        ("Lecture","GEB2404T","2113","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Wednesday,new Time(10,30),new Time(12,20)),
                        }),
                    new Session
                        ("Lecture","GEB2404T","2114","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Wednesday,new Time(15,30),new Time(17,20)),
                        }),
                },
                // DDA 4230
                new List<Session>
                {
                    new Session
                        ("Lecture","DDA4230","2059","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(13,30),new Time(14,50)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(13,30),new Time(14,50)),
                        }),
                },
                // CSC 3170
                new List<Session>
                {
                    new Session
                        ("Lecture","CSC3170","1616","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(10,30),new Time(11,50)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(10,30),new Time(11,50)),
                        }),
                    new Session
                        ("Lecture","CSC3170","1617","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(13,30),new Time(14,50)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(13,30),new Time(14,50)),
                        }),
                },
                // RMS 4060
                new List<Session>
                {
                    new Session
                        ("Lecture","RMS4060","1737","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(15,30),new Time(16,50)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(15,30),new Time(16,50)),
                        }),
                },
                // CSC4020
                new List<Session>
                {
                    new Session
                        ("Lecture","CSC4020","1642","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(8,30),new Time(9,50)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(8,30),new Time(9,50)),
                        }),
                },
                // ERG3020
                new List<Session>
                {
                    new Session
                        ("Lecture","ERG3020","1744","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(10,30),new Time(11,50)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(10,30),new Time(11,50)),
                        }),
                },
                // STA3010
                new List<Session>
                {
                    new Session
                        ("Lecture","STA3010","1690","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(9,00),new Time(10,20)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(9,00),new Time(10,20)),
                        }),
                    new Session
                        ("Lecture","STA3010","1691","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(13,30),new Time(14,50)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(13,30),new Time(14,50)),
                        }),
                },
                // CSC 3050
                new List<Session>
                {
                    new Session
                        ("Lecture","CSC3050","1607","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(13,30),new Time(15,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(13,30),new Time(15,50)),
                        }),
                    new Session
                        ("Lecture","CSC3050","1608","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(10,30),new Time(11,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(10,30),new Time(11,50)),
                        }),

                },
                // CSC 4180
                new List<Session>
                {
                    new Session
                        ("Lecture","CSC3050","1607","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Tuesday,new Time(10,30),new Time(11,50)),
                            new SessionTime(DayOfWeek.Thursday,new Time(10,30),new Time(11,50)),
                        }),

                },
                // EIE 2810
                new List<Session>
                {
                    new Session
                        ("Lecture","EIE2810","1781","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Thursday,new Time(15,00),new Time(17,50)),
                        }),
                    new Session
                        ("Lecture","EIE2810","1782","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Friday,new Time(9,00),new Time(11,50)),
                        }),

                },
                // GEB 2405
                new List<Session>
                {
                    new Session
                        ("Lecture","GEB2405","1969","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Friday,new Time(8,30),new Time(9,20)),
                        }),
                    new Session
                        ("Lecture","GEB2405","1970","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Friday,new Time(10,30),new Time(11,20)),
                        }),
                    new Session
                        ("Lecture","GEB2405","1971","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Friday,new Time(14,30),new Time(14,50)),
                        }),
                },
                // ENG2002S
                new List<Session>
                {
                    new Session
                        ("Lecture","ENG2002S","1821","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(9,00),new Time(10,20)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(9,00),new Time(10,20)),
                        }),
                    new Session
                        ("Lecture","ENG2002S","1691","staff", "TBA",
                        new List<SessionTime>
                        {
                            new SessionTime(DayOfWeek.Monday,new Time(13,30),new Time(14,50)),
                            new SessionTime(DayOfWeek.Wednesday,new Time(13,30),new Time(14,50)),
                        }),
                },
                // ENG2002B
                new List<Session>
                    {
                        new Session
                            ("Lecture","ENG2002B","????","staff", "TBA",
                            new List<SessionTime>
                            {
                                new SessionTime(DayOfWeek.Tuesday,new Time(15,30),new Time(16,50)),
                                new SessionTime(DayOfWeek.Thursday,new Time(15,30),new Time(16,50)),
                            }),
                    },
            };
            #endregion

            List<string> courseToSelect = new() { "FIN3080", "FIN4110", "MAT2002", "ENG2002B" };

            List<Course> courses = allLectures
                .Where(c => courseToSelect.Contains(c.First().Name))
                .Select(c => new Course(c, Priority.Required)).ToList();

            courses[0].Priority = Priority.Preferred;
            courses[1].Priority = Priority.Optional;

            IPriorityQueue<Schedule, Schedule.PriorityValue> result = ClassSelector.FindSchedules(courses);
            Console.WriteLine(result.First());
        }
    }
}
