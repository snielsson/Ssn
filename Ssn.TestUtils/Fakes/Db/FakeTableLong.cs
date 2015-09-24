using System;
using System.Threading;
namespace Ssn.TestUtils.Fakes.Db {
    public class FakeTableLong<T> : FakeTable<long, T> {
        private long nextKey;
        public FakeTableLong(Func<T, long> GetKey, Action<T, long> SetKey) {
            this.GetKey = GetKey;
            this.SetKey = SetKey;
            NextKey = () => Interlocked.Increment(ref nextKey);
        }
    }
}