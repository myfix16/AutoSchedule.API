using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using AutoSchedule.Core.Models;
using CsvHelper;

namespace AutoSchedule.Core.Helpers
{
    public class CsvDataProvider : IDataProvider<IEnumerable<Session>>
    {
        readonly string _csvDirectionPath;

        public CsvDataProvider(string csvPath) => _csvDirectionPath = csvPath;

        public IEnumerable<Session> GetSessions()
        {
            List<Session> sessions = new();
            foreach (string path in Directory.GetFiles(_csvDirectionPath, "*.csv"))
            {
                sessions.AddRange(ReadSessions(path));
            }
            return sessions;
        }

        [Obsolete("This is actually a fake async method using await new Task.")]
        public async Task<IEnumerable<Session>> GetSessionsAsync()
        {
            return await new Task<IEnumerable<Session>>(() => ReadSessions(_csvDirectionPath));
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
                    string[] splittedTime = timeString.Split(' ');
                    // Do something.
                    for (int i = 0; i < splittedTime[0].Length; i += 2)
                    {
                        DayOfWeek dayOfWeek = splittedTime[0][i..(i + 2)] switch
                        {
                            "Mo" => DayOfWeek.Monday,
                            "Tu" => DayOfWeek.Tuesday,
                            "We" => DayOfWeek.Wednesday,
                            "Th" => DayOfWeek.Thursday,
                            "Fr" => DayOfWeek.Friday,
                            _ => throw new NotImplementedException("No such weekday.")
                        };
                        sessionTimes.Add(new SessionTime(dayOfWeek, new Time(splittedTime[1]), new Time(splittedTime[3])));
                    }
                }
                // 2. split each item such as WeFr 10:00-11:00
                sessions.Add(new Session(sessionType, name, codeField[8..12], instructor, location, sessionTimes));
            }
            return sessions;
        }
    }
}
