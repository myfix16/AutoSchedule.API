using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json.Serialization;
using AutoSchedule.Core.Helpers;

namespace AutoSchedule.Core.Models
{
    /// <summary>
    /// A schedule that contains all class selected.
    /// </summary>
    [Serializable]
    public class Schedule : IEquatable<Schedule>
    {
        [JsonIgnore]
        static readonly int[,] LocationDistance = new int[9, 9]
        {
            { 0, 3, 1, 15, 14, 13, 12, 18, 15 },
            { 0, 0, 3, 13, 12, 11, 10, 19, 16 },
            { 0, 0, 0, 16, 15, 14, 13, 16, 13 },
            { 0, 0, 0, 0, 1, 2, 3, 5, 4 },
            { 0, 0, 0, 0, 0, 1, 2, 6, 5 },
            { 0, 0, 0, 0, 0, 0, 1, 7, 6 },
            { 0, 0, 0, 0, 0, 0, 0, 8, 7 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 3 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        [JsonIgnore]
        static readonly double SpeedThreshold;

        static Schedule()
        {
            // fill in the symmetric part of LocationDistance
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < row; col++)
                {
                    LocationDistance[row, col] = LocationDistance[col, row];
                }
            }

            // todo: find a good speed threshold
            // use the distance between TA and ChengDao / 15 min as the threshold of bad schedule
            SpeedThreshold = (double)LocationDistance[0, 3] / 15;
        }

        [JsonInclude]
        public string Id { get; set; } = "1";

        [Serializable]
        public struct PriorityValue : IComparable<PriorityValue>
        {
            public int PreferredNum, OptionalNum, LocationPriority;

            /// <summary>
            /// Compare the priority value.
            /// </summary>
            /// <param name="other"></param>
            /// <returns>
            /// positive: other has higher priority value
            /// 0: the same
            /// negative: this has higher priority value
            /// </returns>
            public int CompareTo(PriorityValue other)
            {
                // 1st rule: select schedules with most preferred and then optional classes
                int classNumResult = CompareClassNum(other);
                if (classNumResult != 0) return classNumResult;
                // 2nd rule: select schedules without adjacent sessions whose locations are far from each other
                return other.LocationPriority - LocationPriority;
            }

            private int CompareClassNum(PriorityValue other)
            {
                int result = other.PreferredNum - PreferredNum;
                return other.PreferredNum - PreferredNum == 0 ? other.OptionalNum - OptionalNum : result;
            }
        }

        [JsonIgnore]
        public PriorityValue Priority = new() { PreferredNum = 0, OptionalNum = 0, LocationPriority = 0 };

        [JsonInclude]
        public ObservableCollection<Session> Sessions { get; set; } = new();

        public Schedule() { }

        [JsonConstructor]
        public Schedule(string id, IEnumerable<Session> sessions)
        {
            Id = id;
            Sessions = new ObservableCollection<Session>(sessions);
        }


        /// <summary>
        /// Validate whether one session can be successfully added.
        /// That is, whether there is any time conflict of sessions.
        /// </summary>
        /// <param name="newSession"></param>
        /// <returns>true if newSession can be added, false otherwise.</returns>
        public bool Validate(Session newSession) => Sessions.All(session => !session.HasConflictSession(newSession));

        public Schedule WithAdded(Session element, Priority priority)
        {
            Schedule newSchedule = ShallowCopy();
            newSchedule.Sessions.Add(element);
            // update newSchedule's PreferredNum and OptionalNum
            switch (priority)
            {
                case Models.Priority.Required:
                    break;
                case Models.Priority.Preferred:
                    newSchedule.Priority.PreferredNum++;
                    break;
                case Models.Priority.Optional:
                    newSchedule.Priority.OptionalNum++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
            }

            // update newSchedule's LocationPriority
            newSchedule.Priority.LocationPriority = newSchedule.GetLocationPriority();

            return newSchedule;
        }

        public Schedule ShallowCopy() => new()
        {
            Id = Id,
            Priority = Priority,
            Sessions = new ObservableCollection<Session>(Sessions),
        };

        /// <summary>
        /// Currently, this will return a negative int if there are adjacent classes located far away.
        /// It will return 0 otherwise.
        /// </summary>
        /// <returns></returns>
        int GetLocationPriority()
        {
            if (Sessions.Count == 1) return 0;

            int priority = 0;

            var splitSessions = Sessions.SelectMany(s => s.SessionTimes,
                (s, t) => new
                {
                    SessionType = s.SessionType,
                    Location = s.Location,
                    Time = t
                }).OrderBy(s => s.Time.StartTimeFromMon).ToList();

            // if there are two adjacent sessions whose locations are far from each other, priority--;
            for (int i = 0, iMax = splitSessions.Count - 1; i < iMax; ++i)
            {
                var lastClass = splitSessions[i];
                var nextClass = splitSessions[i + 1];
                ClassRoom lastRoom = ClassRoomParser.FromString(lastClass.Location);
                ClassRoom nextRoom = ClassRoomParser.FromString(nextClass.Location);
                double speed = (double)LocationDistance[(int)lastRoom, (int)nextRoom] /
                               (nextClass.Time.StartTimeFromMon - lastClass.Time.EndTimeFromMon);
                if (speed > SpeedThreshold) --priority;
            }

            return priority;
        }

        void UpdateLocationPriority(object sender, NotifyCollectionChangedEventArgs e) => Priority.LocationPriority = GetLocationPriority();

        public bool Equals(Schedule other)
        {
            if (other == null) return false;
            return ReferenceEquals(this, other) || Sessions.SequenceEqual(other.Sessions);
        }

        public static bool operator ==(Schedule s1, Schedule s2) => s1 is null ? s2 is null : s1.Equals(s2);
        public static bool operator !=(Schedule s1, Schedule s2) => !(s1 == s2);
    }
}