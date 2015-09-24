// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Threading.Tasks;
namespace Ssn.Utils.Extensions {
    public static class TaskExtensions {
        public static void HandleExceptions(this Task task, Action<Exception> Handler) {
            task.ContinueWith(t => {
                if (t != null && t.Exception != null) {
                    AggregateException aggException = t.Exception.Flatten();
                    foreach (Exception exception in aggException.InnerExceptions) {
                        Handler(exception);
                    }
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}