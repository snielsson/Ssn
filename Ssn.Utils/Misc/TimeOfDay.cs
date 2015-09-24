// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
namespace Ssn.Utils.Misc {
    public class TimeOfDay {
        protected bool Equals(TimeOfDay other) {
            return _timeSpan.Equals(other._timeSpan);
        }
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TimeOfDay) obj);
        }
        public override int GetHashCode() {
            return _timeSpan.GetHashCode();
        }
        private readonly TimeSpan _timeSpan;
        public TimeOfDay() {}
        public TimeOfDay(DateTime dateTime) {
            _timeSpan = new TimeSpan(0, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        }

        public static bool operator <(TimeOfDay lhs, TimeOfDay rhs) {
            return lhs._timeSpan < rhs._timeSpan;
        }
        public static bool operator >(TimeOfDay lhs, TimeOfDay rhs) {
            return lhs._timeSpan > rhs._timeSpan;
        }
        public static bool operator ==(TimeOfDay lhs, TimeOfDay rhs) {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs), @"Left hand side of equality operator is null");
            if (rhs == null) throw new ArgumentNullException(nameof(rhs), @"Right hand side of equality operator is null");
            return lhs._timeSpan == rhs._timeSpan;
        }
        public static bool operator !=(TimeOfDay lhs, TimeOfDay rhs) {
            return !(lhs == rhs);
        }

        public static implicit operator TimeOfDay(DateTime dateTime) {
            return new TimeOfDay(dateTime);
        }
        public static implicit operator TimeSpan(TimeOfDay timeOfDay) {
            return timeOfDay._timeSpan;
        }
    }
}