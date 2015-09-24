// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
using Ssn.Utils.Misc;
namespace Ssn.Utils.Extensions {
    /// <summary>
    /// Various DateTime utility functions.
    /// </summary>
    public static class DateTimeExtensions {
        /// <summary>
        /// Returns a new DateTime which is the given DateTime rounded down to the nearest whole TimeSpan period.
        /// </summary>
        /// <param name="this">The DateTime to round.</param>
        /// <param name="timeSpanTicks">Length in ticks of the the period to round to.</param>
        /// <param name="numberOfPeriodsToRoundTo">Optional number of periods to round down to - negative numbers also allowed.</param>
        /// <returns>A new DateTime with a rounded value.</returns>
        public static DateTime RoundToTimeSpanStart(this DateTime @this, long timeSpanTicks, int numberOfPeriodsToRoundTo = 0) {
            return new DateTime(timeSpanTicks*(@this.Ticks/timeSpanTicks + numberOfPeriodsToRoundTo));
        }
        /// <summary>
        /// Returns a new DateTime which is the given DateTime rounded down to the nearest whole TimeSpan period.
        /// </summary>
        /// <param name="this">The DateTime to round.</param>
        /// <param name="timeSpanTicks">Length in ticks of the the period to round to.</param>
        /// <param name="numberOfPeriodsToRoundTo">Optional number of periods to round down to - negative numbers also allowed.</param>
        /// <returns>A new DateTime with a rounded value.</returns>
        public static DateTime RoundToTimeSpanEnd(this DateTime @this, long timeSpanTicks, int numberOfPeriodsToRoundTo = 0) {
            return RoundToTimeSpanStart(@this, timeSpanTicks, 1 + numberOfPeriodsToRoundTo);
        }
        /// <summary>
        /// Returns a new DateTime which is the given DateTime rounded down to the nearest whole TimeSpan period.
        /// </summary>
        /// <param name="this">The DateTime to round.</param>
        /// <param name="period">The period to round to.</param>
        /// <param name="numberOfPeriodsToRoundTo">Optional number of periods to round down to - negative numbers also allowed.</param>
        /// <returns>A new DateTime with a rounded value.</returns>
        public static DateTime RoundToTimeSpanEnd(this DateTime @this, TimeSpan period, int numberOfPeriodsToRoundTo = 0) {
            return RoundToTimeSpanStart(@this, period.Ticks, 1 + numberOfPeriodsToRoundTo);
        }
        /// <summary>
        /// Returns a new DateTime which is the given DateTime rounded down to the nearest whole TimeSpan period.
        /// </summary>
        /// <param name="this">The DateTime to round.</param>
        /// <param name="period">The period to round to.</param>
        /// <param name="numberOfPeriodsToRoundTo">Optional number of periods to round down to - negative numbers also allowed.</param>
        /// <returns>A new DateTime with a rounded value.</returns>
        public static DateTime RoundToTimeSpanStart(this DateTime @this, TimeSpan period, int numberOfPeriodsToRoundTo = 0) {
            return RoundToTimeSpanStart(@this, period.Ticks, numberOfPeriodsToRoundTo);
        }
        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// Returns true if the given DateTime is in the future. The method to determine the value of 'now' can be customized, but defaults to System.DateTime.UtcNow.
        /// </summary>
        /// <param name="this">The DateTime to compare with 'now'.</param>
        /// <param name="getNow">The method for getting the value of 'now', defaults to System.DateTime.UtcNow.</param>
        /// <returns>True if the given DateTime is greater than 'now'.</returns>
        public static bool IsInTheFuture(this DateTime @this, Func<DateTime> getNow = null) {
            DateTime now = getNow == null ? DateTime.UtcNow : getNow();
            return @this.Ticks > now.Ticks;
        }
        /// <summary>
        /// Returns true if the given DateTime is in the past. The method to determine the value of 'now' can be customized, but defaults to System.DateTime.UtcNow.
        /// </summary>
        /// <param name="this">The DateTime to compare with 'now'.</param>
        /// <param name="getNow">The method for getting the value of 'now', defaults to System.DateTime.UtcNow.</param>
        /// <returns>True if the given DateTime is less than 'now'.</returns>
        public static bool IsInThePast(this DateTime @this, Func<DateTime> getNow = null) {
            DateTime now = getNow == null ? DateTime.UtcNow : getNow();
            return @this.Ticks < now.Ticks;
        }
        /// <summary>
        /// Returns the given DateTime converted to the number of seconds elapsed since the EPOCH of January the 1st. 1970.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>An int representing the number of elapsed seconds since January the 1st. 1970.</returns>
        public static int ToUnixTime(this DateTime @this) {
            return (int) (@this - EPOCH).TotalSeconds;
        }
        /// <summary>
        /// Converts the Unix time (number of elapsed seconds since January 1st. 1970) to a DateTime with kind Utc.
        /// </summary>
        /// <param name="this">The Unix time (a number of seconds) to convert to an UTC DateTime.</param>
        /// <returns>The DateTime represention of the Unix time. The DateTime has kind Utc.</returns>
        public static DateTime FromUnixTime(this int @this) {
            return EPOCH.AddSeconds(@this);
        }
        /// <summary>
        /// Returns the given DateTime converted to the number of seconds elapsed since the EPOCH of January the 1st. 1970.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>A long representing the number of elapsed milliseconds since January the 1st. 1970.</returns>
        public static long ToUnixTimeMs(this DateTime @this) {
            return (long) (@this - EPOCH).TotalMilliseconds;
        }
        /// <summary>
        /// Converts the Unix time with millisecond resolution (number of elapsed milliseconds since January 1st. 1970) to a DateTime with kind Utc.
        /// </summary>
        /// <param name="this">The Unix time with millisecond resolution (a number of milliseconds) to convert to an UTC DateTime.</param>
        /// <returns>The DateTime represention of the Unix time with millisecond resolution. The DateTime has kind Utc.</returns>
        public static DateTime FromUnixTimeMs(this long @this) {
            return EPOCH.AddMilliseconds(@this);
        }

        public static bool NotOlderThan(this DateTime @this, TimeSpan timeSpan, DateTime now = default(DateTime)) {
            if (now == default(DateTime)) now = DateTime.UtcNow;
            var dateTime = now.Add(timeSpan);
            return NotBefore(@this, dateTime);
        }
        public static bool NotOlderThan(this Date @this, TimeSpan timeSpan, DateTime now = default(DateTime)) {
            if (now == default(DateTime)) now = DateTime.UtcNow;
            var dateTime = now.Add(timeSpan);
            return NotBefore(@this, dateTime);
        }
        public static bool NotBefore(this DateTime @this, DateTime dateTime) {
            return @this >= dateTime;
        }
        public static bool NotBefore(this Date @this, DateTime dateTime) {
            return @this >= dateTime;
        }
        public static bool IsOlderThan(this DateTime @this, TimeSpan timeSpan, DateTime now = default(DateTime)) {
            if (now == default(DateTime)) now = DateTime.UtcNow;
            var dateTime = now.Add(timeSpan);
            return IsBefore(@this, dateTime);
        }
        public static bool IsOlderThan(this Date @this, TimeSpan timeSpan, DateTime now = default(DateTime)) {
            if (now == default(DateTime)) now = DateTime.UtcNow;
            var dateTime = now.Add(timeSpan);
            return IsBefore(@this, dateTime);
        }
        public static bool IsBefore(this DateTime @this, DateTime dateTime) {
            return @this < dateTime;
        }
        public static bool IsBefore(this Date @this, DateTime dateTime) {
            return (DateTime) @this < dateTime;
        }
    }
}