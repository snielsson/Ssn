using System;
namespace Ssn.TestUtils.Fakes.Db {
    public class FakeTableString<T> : FakeTable<string, T> {
        public FakeTableString(Func<T, string> GetKey, Action<T, string> SetKey) {
            this.GetKey = GetKey;
            this.SetKey = SetKey;
        }
    }
}