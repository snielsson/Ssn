// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
namespace Ssn.Utils.Misc {
    public class KeyVal<TKey, TVal> {
        public TKey Key { get; set; }
        public TVal Val { get; set; }
    }
    public class KeyVal : KeyVal<string, string> {}
}