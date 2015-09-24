// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ssn.Utils.Extensions;

namespace Ssn.TestUtils.Fakes.Db {
    public abstract class FakeTable<Tkey, T> : IFakeTable where Tkey : IComparable<Tkey> {
        private readonly ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
        protected Func<Tkey> NextKey { get; set; }
        protected Func<T, Tkey> GetKey { get; set; }
        protected Action<T, Tkey> SetKey { get; set; }
        private SortedDictionary<Tkey, T> _table { get; } = new SortedDictionary<Tkey, T>();
        public Task<T> Insert(T entity) {
            try {
                _rw.EnterWriteLock();
                var key = GetKey(entity);
                if (key.HasValue()) throw new Exception("Cannot insert entity with a value in key field.");
                if (_table.ContainsKey(key)) throw new InvalidOperationException("Entity with key " + key + " already in table of type " + typeof (T).Name);
                if (NextKey != null) SetKey(entity, NextKey());
                _table[key] = entity.Clone();
                return Task.FromResult(entity);
            }
            finally {
                _rw.ExitWriteLock();
            }
        }
        public Task<T> Read(Tkey key) {
            try {
                _rw.EnterReadLock();
                T result;
                _table.TryGetValue(key, out result);
                return Task.FromResult(result.Clone());
            }
            finally {
                _rw.ExitReadLock();
            }
        }
        public Task Update(T entity) {
            try {
                _rw.EnterWriteLock();
                var key = GetKey(entity);
                if (!key.HasValue()) throw new Exception("Cannot update entity with no value in key field.");
                if (!_table.ContainsKey(key)) throw new Exception("Cannot update non-existing entity, key = " + key);
                _table[key] = entity;

                return Task.FromResult(0);
            }
            finally {
                _rw.ExitWriteLock();
            }
        }
        public Task Save(T entity) {
            try {
                _rw.EnterWriteLock();
                var key = GetKey(entity);
                if (key.HasValue()) return Update(entity);
                else return Insert(entity);
            }
            finally {
                _rw.ExitWriteLock();
            }
        }
        public Task Delete(T entity) {
            try {
                _rw.EnterWriteLock();
                var key = GetKey(entity);
                _table.Remove(key);
                return Task.FromResult(0);
            }
            finally {
                _rw.ExitWriteLock();
            }
        }
        public IEnumerable<T> All() {
            try {
                _rw.EnterReadLock();
                return _table.Values.Select(x => x.Clone());
            }
            finally {
                _rw.ExitReadLock();
            }
        }
    }
}