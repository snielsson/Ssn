// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
using System.Threading;
namespace Ssn.Utils.Extensions {
    public static class ReaderWriterLockSlimExtensions {
        private sealed class ReadLockToken : IDisposable {
            private ReaderWriterLockSlim _sync;
            public ReadLockToken(ReaderWriterLockSlim sync) {
                _sync = sync;
                sync.EnterReadLock();
            }
            public void Dispose() {
                if (_sync != null) {
                    _sync.ExitReadLock();
                    _sync = null;
                }
            }
        }
        private sealed class WriteLockToken : IDisposable {
            private ReaderWriterLockSlim _sync;
            public WriteLockToken(ReaderWriterLockSlim sync) {
                _sync = sync;
                sync.EnterWriteLock();
            }
            public void Dispose() {
                if (_sync != null) {
                    _sync.ExitWriteLock();
                    _sync = null;
                }
            }
        }

        public static IDisposable ReadLock(this ReaderWriterLockSlim obj) {
            return new ReadLockToken(obj);
        }
        public static IDisposable WriteLock(this ReaderWriterLockSlim obj) {
            return new WriteLockToken(obj);
        }
    }
}