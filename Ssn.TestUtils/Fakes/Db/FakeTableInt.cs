using System;
using System.Threading;
namespace Ssn.TestUtils.Fakes.Db {
    public class FakeTableInt<T> : FakeTable<int, T> {
        private int nextKey;
        public FakeTableInt(Func<T, int> GetKey, Action<T, int> SetKey) {
            this.GetKey = GetKey;
            this.SetKey = SetKey;
            NextKey = () => Interlocked.Increment(ref nextKey);
        }
    }
}