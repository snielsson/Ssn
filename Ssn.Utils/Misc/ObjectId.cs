// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
using System.Threading;
namespace Ssn.Utils.Misc {
    public static class ObjectId {
        private static readonly long _epoch = new DateTime(2015, 4, 7, 0, 0, 0).Ticks;
        private static long _nextId;
        public static long Create(long shardId = 1) {
            var timeStamp = (DateTime.UtcNow.Ticks - _epoch)/TimeSpan.TicksPerMillisecond;
            var result = timeStamp << 23;
            result |= (shardId << 10);
            var nextId = Interlocked.Increment(ref _nextId);
            result |= nextId%1024;
            return result;
        }
        public static DateTime GetTimeStamp(long objectId) {
            var result = new DateTime((objectId >> 23)*TimeSpan.TicksPerMillisecond + _epoch);
            return result;
        }
    }
}