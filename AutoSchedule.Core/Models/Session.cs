using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoSchedule.Core.Models
{
    /// <summary>
    /// A base class of actual class sessions.
    /// </summary>
    [Serializable]
    public class Session
    {
        [JsonInclude]
        public string SessionType;

        /// <summary>
        /// Represents all time of the session. 
        /// </summary>
        /// <remarks>E.g. Mon 8:30-10:20 and Wed 8:30-10:20.</remarks>
        [JsonInclude]
        public List<SessionTime> SessionTimes;

        string _sessionTimesString;

        [JsonIgnore]
        public string SessionTimesString
        {
            get
            {
                _sessionTimesString ??= string.Join(' ', SessionTimes).Trim();
                return _sessionTimesString;
            }
        }

        // ！ requires property instead of field to work properly in SFGrid
        [JsonInclude]
        public string Instructor { get; init; }

        [JsonPropertyName("id")]
        [JsonInclude]
        public string Code { get; init; }

        [JsonInclude]
        public string Name { get; init; }

        [JsonInclude]
        public string Location { get; init; }

        public Session()
        {
            SessionType = string.Empty;
            Name = string.Empty;
            Code = string.Empty;
            Instructor = string.Empty;
            Location = string.Empty;
            SessionTimes = new List<SessionTime>();
        }

        public Session(string sessionType, string name, string code, string instructor,
                       string location, List<SessionTime> sessionTimes)
        {
            SessionType = sessionType;
            Name = name;
            Code = code;
            Instructor = instructor;
            Location = location;
            SessionTimes = sessionTimes;
        }

        public override string ToString()
        {
            var output = $"{Name} {Code} {Instructor}";
            // foreach (var item in SessionTimes) output += $" {item}";
            output += $" {SessionTimesString}";
            return output;
        }

        /// <summary>
        /// Judge whether a new session will have conflict with original sessions.
        /// </summary>
        /// <param name="session2">The new session.</param>
        /// <returns>true if there is time conflict, false otherwise.</returns>
        public bool HasConflictSession(Session session2)
        {
            foreach (SessionTime sessionTime in SessionTimes)
            {
                foreach (SessionTime otherSessionTime in session2.SessionTimes)
                {
                    if (sessionTime.ConflictWith(otherSessionTime)) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return a session-invariant class name. E.g. ACT2111 L01 -> ACT2111 LEC; CSC3001 T05 -> CSC3001 TUT.
        /// </summary>
        /// <returns>A new string representing session-invariant class name</returns>
        public string GetClassifiedName() => $"{Name.Split(' ')[0]} {SessionType}";
    }
}
