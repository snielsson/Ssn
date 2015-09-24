// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Concurrent;
namespace Ssn.TestUtils.Fakes.Db {
    public class FakeDb {
        public ConcurrentDictionary<Type, IFakeTable> Tables { get; set; }
        public IFakeTable GetOrAddTable(IFakeTable fakeTable) {
            return Tables.GetOrAdd(fakeTable.GetType(), fakeTable);
        }
        public IFakeTable GetTable(Type type) {
            return Tables[type];
        }
    }
}