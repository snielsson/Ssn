using System;
namespace Ssn.TestUtils.Fakes.Db {
    public class FakeTableGuid<T> : FakeTable<Guid, T> {
        public FakeTableGuid(Func<T, Guid> GetKey, Action<T, Guid> SetKey) {
            this.GetKey = GetKey;
            this.SetKey = SetKey;
            NextKey = Guid.NewGuid;
        }
    }
}