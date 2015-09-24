// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System.Threading;
namespace Ssn.Utils.Misc {
    public class BooleanWaitEvent {
        public BooleanWaitEvent(bool initialState = false) {
            lock (_lock) {
                _true = new ManualResetEvent(initialState);
                _false = new ManualResetEvent(!initialState);
            }
        }
        private readonly object _lock = new object();
        private readonly ManualResetEvent _true;
        private readonly ManualResetEvent _false;
        public bool IsTrue { get { lock (_lock) return _true.WaitOne(0) && !_false.WaitOne(0); } }
        public bool IsFalse { get { lock (_lock) return !IsTrue; } }

        public void SetTrue() {
            lock (_lock) {
                _false.Reset();
                _true.Set();
            }
        }

        public void SetFalse() {
            lock (_lock) {
                _true.Reset();
                _false.Set();
            }
        }

        public bool WaitUntilTrue(int millisecondsTimeout = -1) {
            return _true.WaitOne(millisecondsTimeout);
        }

        public bool WaitUntilFalse(int millisecondsTimeout = -1) {
            return _false.WaitOne(millisecondsTimeout);
        }
    }
}