﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSchedule.Core.Models
{
    public class Course: IEnumerable<Session>
    {
        public readonly string Name;
        public Priority Priority = Priority.Required;
        public readonly List<Session> Sessions;

        public Course()
        {
            Sessions = new();
        }

        public Course(IEnumerable<Session> sessions, Priority coursePriority)
        {
            Sessions = new List<Session>(sessions);
            Priority = coursePriority;
        }

        public Course(string name, IEnumerable<Session> sessions, Priority coursePriority)
        {
            Name = name;
            Sessions = new List<Session>(sessions);
            Priority = coursePriority;
        }

        public IEnumerator<Session> GetEnumerator() => Sessions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
