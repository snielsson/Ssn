// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
namespace Ssn.Utils.Caching {
    public interface ICacheValue<TKey> where TKey : IComparable {
        TKey Key { get; set; }
    }
}