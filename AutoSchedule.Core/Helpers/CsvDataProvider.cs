using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AutoSchedule.Core.Models;
using CsvHelper;

namespace AutoSchedule.Core.Helpers
{
    public class CsvDataProvider : IDataProvider<IEnumerable<Session>>
    {
        readonly string _csvDirectionPath;

        public CsvDataProvider(string csvPath) => _csvDirectionPath = csvPath;

        public IEnumerable<Session> GetData()
        {
            List<Session> sessions = new();
            foreach (string path in Directory.GetFiles(_csvDirectionPath, "*.csv"))
            {
                sessions.AddRange(ReadSessions(path));
            }
            return sessions;
        }

        private static IEnumerable<Session> ReadSessions(string csvPath)
        {
            var sessions = new List<Session>();
            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                string codeField = csv.GetField("Code");
                string name = $"{csv.GetField("Name").Split('-')[0]} {codeField[0..3]}";
                string sessionType = codeField[4..7];
                string instructor = csv.GetField("Instructor");
                string location = csv.GetField("Location");
                string[] timesField = csv.GetField("Time").Split(';');
                if (timesField[0] == "TBA") continue;
                var sessionTimes = new List<SessionTime>();
                foreach (string timeString in timesField)
                {
                    string[] splitTime = timeString.Split(' ');
                    // Do something.
                    for (int i = 0; i < splitTime[0].Length; i += 2)
                    {
                        DayOfWeek dayOfWeek = splitTime[0][i..(i + 2)] switch
                        {
                            "Mo" => DayOfWeek.Monday,
                            "Tu" => DayOfWeek.Tuesday,
                            "We" => DayOfWeek.Wednesday,
                            "Th" => DayOfWeek.Thursday,
                            "Fr" => DayOfWeek.Friday,
                            _ => throw new NotImplementedException("No such weekday.")
                        };
                        sessionTimes.Add(new SessionTime(dayOfWeek, new Time(splitTime[1]), new Time(splitTime[3])));
                    }
                }
                // 2. split each item such as WeFr 10:00-11:00
                sessions.Add(new Session(sessionType, name, codeField[8..12], instructor, location, sessionTimes));
            }
            return sessions;
        }
    }
}
