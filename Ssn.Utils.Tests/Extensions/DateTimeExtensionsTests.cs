// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using NUnit.Framework;
using System;
using Ssn.Utils.Extensions;

namespace Ssn.Utils.Tests.Extensions {
    public class DateTimeExtensionsTests {
        private DateTime _dateTime = new DateTime(2015, 1, 1, 1, 1, 1, 1);
        private DateTime _localDateTime = new DateTime(2015, 1, 1, 1, 1, 1, 1, DateTimeKind.Local);
        private DateTime _utcDateTime = new DateTime(2015, 1, 1, 1, 1, 1, 1, DateTimeKind.Utc);
        private DateTime _unspecifiedDateTime = new DateTime(2015, 1, 1, 1, 1, 1, 1, DateTimeKind.Unspecified);
        [Test]
        public void RoundToTimeSpanStartWorks() {
            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 0, 0, 0), _dateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1)));
            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1)));
            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1)));
            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1)));

            Assert.AreEqual(new DateTime(2015, 1, 1, 2, 0, 0, 0), _dateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), 1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), 1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), 1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), 1));

            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 0, 0), _dateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), -1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), -1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), -1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanStart(TimeSpan.FromHours(1), -1));
        }
        [Test]
        public void RoundToTimeSpanEndWorks() {
            Assert.AreEqual(new DateTime(2015, 1, 1, 2, 0, 0, 0), _dateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1)));
            Assert.AreEqual(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1)));
            Assert.AreEqual(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1)));
            Assert.AreEqual(new DateTime(2015, 1, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1)));

            Assert.AreEqual(new DateTime(2015, 1, 1, 3, 0, 0, 0), _dateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), 1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 3, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), 1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), 1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 3, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), 1));

            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 0, 0, 0), _dateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), -1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Local), _localDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), -1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), _utcDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), -1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), _unspecifiedDateTime.RoundToTimeSpanEnd(TimeSpan.FromHours(1), -1));
        }
        [Test]
        public void IsInTheFutureWorks() {
            DateTime lastYear = DateTime.Now.AddYears(-1);
            DateTime nextYear = DateTime.Now.AddYears(1);
            Assert.False(lastYear.IsInTheFuture());
            Assert.True(nextYear.IsInTheFuture());
            Assert.False(nextYear.IsInTheFuture(() => DateTime.Now.AddYears(2)));
            Assert.True(lastYear.IsInTheFuture(() => DateTime.Now.AddYears(-2)));
            Assert.False(lastYear.IsInTheFuture(() => lastYear));
            Assert.False(nextYear.IsInTheFuture(() => nextYear));
        }
        [Test]
        public void IsInThePastWorks() {
            DateTime lastYear = DateTime.Now.AddYears(-1);
            DateTime nextYear = DateTime.Now.AddYears(1);
            Assert.True(lastYear.IsInThePast());
            Assert.False(nextYear.IsInThePast());
            Assert.True(nextYear.IsInThePast(() => DateTime.Now.AddYears(2)));
            Assert.False(lastYear.IsInThePast(() => DateTime.Now.AddYears(-2)));
            Assert.False(lastYear.IsInThePast(() => lastYear));
            Assert.False(nextYear.IsInThePast(() => nextYear));
        }
        [Test]
        public void ToAndFromUnixTimeWorks() {
            DateTime now = DateTime.UtcNow;
            DateTime nowRoundedToSeconds = now.RoundToTimeSpanStart(TimeSpan.FromSeconds(1));
            int unixTime = now.ToUnixTime();
            Assert.AreEqual(nowRoundedToSeconds, DateTimeExtensions.EPOCH.AddTicks(new DateTime(unixTime*TimeSpan.TicksPerSecond).Ticks));
            Assert.AreEqual(nowRoundedToSeconds, unixTime.FromUnixTime());
        }
        [Test]
        public void ToAndFromUnixTimeMsWorks() {
            DateTime now = DateTime.UtcNow;
            DateTime nowRoundedToMilliSeconds = now.RoundToTimeSpanStart(TimeSpan.FromMilliseconds(1));
            long unixTimeMs = now.ToUnixTimeMs();
            Assert.AreEqual(nowRoundedToMilliSeconds, DateTimeExtensions.EPOCH.AddTicks(new DateTime(unixTimeMs*TimeSpan.TicksPerMillisecond).Ticks));
            Assert.AreEqual(nowRoundedToMilliSeconds, unixTimeMs.FromUnixTimeMs());
        }
    }
}