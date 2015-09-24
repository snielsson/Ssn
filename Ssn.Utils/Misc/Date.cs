// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
using System.Globalization;
namespace Ssn.Utils.Misc {
    public class Date {
        public string ToString(string formatString) {
            return _dateTime.ToString(formatString);
        }
        public string ToString(string formatString, IFormatProvider formatProvider) {
            return _dateTime.ToString(formatString, formatProvider);
        }
        public string ToString(IFormatProvider formatProvider) {
            return _dateTime.ToString(formatProvider);
        }
        public string ToString(CultureInfo cultureInfo) {
            return _dateTime.ToString(cultureInfo);
        }
        public override string ToString() {
            return _dateTime.ToString();
        }
        protected bool Equals(Date other) {
            return _dateTime.Equals(other._dateTime);
        }
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Date) obj);
        }
        public override int GetHashCode() {
            return _dateTime.GetHashCode();
        }
        private readonly DateTime _dateTime;
        public Date() {
            _dateTime = default(DateTime).Date;
        }
        public Date(DateTime dateTime) {
            _dateTime = dateTime.Date;
        }

        public static bool operator <(Date lhs, Date rhs) {
            return lhs._dateTime < rhs._dateTime;
        }
        public static bool operator >(Date lhs, Date rhs) {
            return lhs._dateTime > rhs._dateTime;
        }
        public static bool operator ==(Date lhs, Date rhs) {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs), @"Left hand side of equality operator is null");
            if (rhs == null) throw new ArgumentNullException(nameof(rhs), @"Right hand side of equality operator is null");
            return lhs._dateTime == rhs._dateTime;
        }
        public static bool operator !=(Date lhs, Date rhs) {
            return !(lhs == rhs);
        }

        public static implicit operator Date(DateTime dateTime) {
            return new Date(dateTime);
        }
        public static implicit operator DateTime(Date date) => date._dateTime;
    }
}