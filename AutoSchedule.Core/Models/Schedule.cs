using System;
using System.Collections.ObjectModel;

namespace AutoSchedule.Core.Models
{
    /// <summary>
    /// A schedule that contains all class selected.
    /// </summary>
    [Serializable]
    public class Schedule : ICopyable<Schedule>
    {
        [System.Text.Json.Serialization.JsonInclude]
        [Newtonsoft.Json.JsonRequired]
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

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public PriorityValue Priority = new() { Preferred = 0, Optional = 0 };

        [System.Text.Json.Serialization.JsonInclude]
        [Newtonsoft.Json.JsonRequired]
        public ObservableCollection<Session> Sessions = new();

        /// <summary>
        /// Validate whether one session can be successfully added.
        /// That is, whether there is any time conflict of sessions.
        /// </summary>
        /// <param name="newSession"></param>
        /// <returns>true if newSession can be added, false otherwise.</returns>
        public bool Validate(Session newSession)
        {
            foreach (var session in Sessions)
            {
                if (session.HasConflictSession(newSession))
                    return false;
            }

            return true;
        }

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
            Id = this.Id,
            Sessions = new ObservableCollection<Session>(this.Sessions),
        };

        [Obsolete("Deep copy is not available.")]
        public Schedule DeepCopy()
        {
            throw new NotImplementedException("Deep copy is not available.");
        }
    }
}