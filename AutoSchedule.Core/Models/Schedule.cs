﻿using System;
using System.Collections.Generic;
using AutoSchedule.Core.Helpers;

namespace AutoSchedule.Core.Models
{
    /// <summary>
    /// A schedule that contains all class selected.
    /// </summary>
    [Serializable]
    public class Schedule : ICopyable<Schedule>
    {
        public string ID { get; set; }

        // TODO: Refactor schedule. There is no need to provide corresponding tutorial service. User can just add tutorial name to the list to select them.
        public List<Session> Lectures { get; set; } = new List<Session>();

        public List<Session> Tutorials { get; set; } = new List<Session>();

        /// <summary>
        /// Validate whether one session can be successfully added.
        /// That is, whether there is any time conflict of sessions.
        /// </summary>
        /// <param name="newClass"></param>
        /// <returns></returns>
        public static bool Validate(Session newSession, IEnumerable<Session> existingSessions)
        {
            foreach (var session in existingSessions)
            {
                if (session.HasConflictSession(newSession))
                    return false;
            }
            return true;
        }

        public bool Validate(Session newSession)
        {
            foreach (var session in newSession.SessionType switch
            {
                "Lecture" => Lectures,
                "Tutorial" => Tutorials,
                _ => throw new NotImplementedException()
            })

            if (session.HasConflictSession(newSession))
                return false;

            return true;
        }

        /// <summary>
        /// Add a new session into the schedule.
        /// </summary>
        /// <returns>true if add is successful, false otherwise</returns>
        public static void AddSession(Session newSession, List<Session> existingSessions)
            => existingSessions.Add(newSession);

        public void AddSession(Session newSession)
        {
            var target = newSession.SessionType switch
            {
                "Lecture" => Lectures,
                "Tutorial" => Tutorials,
                _ => throw new NotImplementedException()
            };
            target.Add(newSession);
        }

        public Schedule WithAdded(Session element)
        {
            var newSchedule = ShallowCopy();
            newSchedule.AddSession(element);
            return newSchedule;
        }

        public Schedule ShallowCopy()
        {
            return new Schedule
            {
                ID = this.ID,
                Lectures = new List<Session>(this.Lectures),
                Tutorials = new List<Session>(this.Tutorials),
            };
        }

        public Schedule DeepCopy()
        {
            throw new NotImplementedException();
        }
    }
}
