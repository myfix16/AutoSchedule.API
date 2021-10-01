using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace AutoSchedule.Core.Models
{
    /// <summary>
    /// A schedule that contains all class selected.
    /// </summary>
    [Serializable]
    public class Schedule : ICopyable<Schedule>
    {
        [JsonInclude]
        public string Id = "1";
        
        public struct PriorityValue : IComparable<PriorityValue>
        {
            public int Preferred, Optional;

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
                int result = other.Preferred - Preferred;
                return other.Preferred - Preferred == 0 ? other.Optional - Optional : result;
            }
        }

        [JsonIgnore]
        public PriorityValue Priority = new() { Preferred = 0, Optional = 0 };

        [JsonInclude]
        public ObservableCollection<Session> Sessions = new();

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
            switch (priority)
            {
                case Models.Priority.Required:
                    break;
                case Models.Priority.Preferred:
                    newSchedule.Priority.Preferred++;
                    break;
                case Models.Priority.Optional:
                    newSchedule.Priority.Optional++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
            }

            return newSchedule;
        }

        public Schedule ShallowCopy() => new()
        {
            Id = Id,
            Sessions = new ObservableCollection<Session>(Sessions),
        };

        [Obsolete("Deep copy is not available.")]
        public Schedule DeepCopy()
        {
            throw new NotImplementedException("Deep copy is not available.");
        }

        int GetLocationPriority()
        {
            if (Sessions.Count == 1) return 1;
            // todo: fill in GetLocationPriority
            List<Session> sessions = Sessions.OrderBy(s => s.SessionTimes[0].StartTimeFromMon)
                                             .ToList();
            for (int i = 1; i < sessions.Count; i++)
            {
                
            }
            throw new NotImplementedException();
        }
    }
}