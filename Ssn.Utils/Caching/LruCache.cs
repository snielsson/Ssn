// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Ssn.Utils.Caching {
    public sealed class LruCache<TKey, TValue> 
        where TValue : ICacheValue<TKey>
        where TKey : IComparable {
        public LruCache() {}
        public LruCache(IEnumerable<TValue> values) {
            foreach (TValue value in values) {
                Add(value);
            }
        }
        private readonly int _limit = int.MaxValue;
        private readonly LinkedList<TValue> _linkedValues = new LinkedList<TValue>();
        private readonly Dictionary<TKey, LinkedListNode<TValue>> _dict = new Dictionary<TKey, LinkedListNode<TValue>>();
        public LruCache(int limit) {
            _limit = limit;
        }

        public TValue this[TKey key] {
            get {
                lock (_dict) {
                    LinkedListNode<TValue> node;
                    return _dict.TryGetValue(key, out node) ? node.Value : default(TValue);
                }
            }
            set {
                lock (_dict) {
                    Add(value);
                }
            }
        }

        public void Add(TValue val) {
            lock (_dict) {
                LinkedListNode<TValue> existing;
                if (_dict.TryGetValue(val.Key, out existing)) {
                    _dict.Remove(val.Key);
                    _linkedValues.Remove(existing);
                }
                else Trim();
                _dict[val.Key] = _linkedValues.AddFirst(val);
            }
        }

        public void Trim(int count = 0) {
            lock (_dict) {
                int numberOfItemsToRemove = Math.Min(_dict.Count, _dict.Count + count - _limit);
                while (numberOfItemsToRemove > 0) {
                    LinkedListNode<TValue> last = _linkedValues.Last;
                    bool wasRemoved = _dict.Remove(last.Value.Key);
                    Debug.Assert(wasRemoved);
                    _linkedValues.Remove(last);
                    numberOfItemsToRemove--;
                }
            }
        }
    }
}