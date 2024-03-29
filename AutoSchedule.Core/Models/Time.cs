﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AutoSchedule.Core.Models
{
    [Serializable]
    public readonly struct Time : IComparable<Time>, IEquatable<Time>
    {
        [JsonInclude]
        [Required]
        public readonly int Hour;

        [JsonInclude]
        [Required]
        public readonly int Minute;

        /// <summary>
        /// Represents the time span from 00:00 to this time counted in minutes.
        /// </summary>
        [JsonIgnore]
        public readonly int TotalMinutes;

        /// <summary>
        /// Construct the time by a string.
        /// </summary>
        /// <param name="timeString">A string representation of time, for example, 10:30.</param>
        public Time(string timeString)
        {
            string[] splitString = timeString.Replace(" ", string.Empty).Split(':');
            Hour = int.Parse(splitString[0]);
            Minute = int.Parse(splitString[1]);
            TotalMinutes = (Hour * 60) + Minute;
        }

        [JsonConstructor]
        public Time(int hour, int minute)
        {
            if (hour is < 0 or > 23)
                throw new ArgumentOutOfRangeException(nameof(hour), "The number of hour must be within [0,23].");

            if (minute is < 0 or > 59)
                throw new ArgumentOutOfRangeException(nameof(minute), "The number of minute must be within [0,59].");

            Hour = hour;
            Minute = minute;
            TotalMinutes = (Hour * 60) + Minute;
        }

        public int CompareTo(Time other) => TotalMinutes - other.TotalMinutes;

        public bool Equals(Time other) => TotalMinutes == other.TotalMinutes;

        public override bool Equals(object obj) => obj is Time other && Equals(other);

        public override int GetHashCode() => TotalMinutes.GetHashCode();

        public override string ToString() => $"{Hour}:{(Minute == 0 ? "00" : Minute)}";

        #region Operators

        public static bool operator >(Time t1, Time t2) => t1.TotalMinutes > t2.TotalMinutes;

        public static bool operator <(Time t1, Time t2) => t1.TotalMinutes < t2.TotalMinutes;

        public static bool operator <=(Time t1, Time t2) => t1.TotalMinutes >= t2.TotalMinutes;

        public static bool operator >=(Time t1, Time t2) => t1.TotalMinutes >= t2.TotalMinutes;

        public static bool operator ==(Time t1, Time t2) => t1.TotalMinutes == t2.TotalMinutes;

        public static bool operator !=(Time t1, Time t2) => t1.TotalMinutes != t2.TotalMinutes;

        #endregion
    }
}