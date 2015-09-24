// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Generic;
using System.Linq;
namespace Ssn.Utils.Extensions {
    public static class EnumExtensions {
        public static IEnumerable<Enum> GetFlags(this Enum flags) {
            var flag = 1ul;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>()) {
                var bits = Convert.ToUInt64(value);
                while (flag < bits) {
                    flag <<= 1;
                }
                if (flag == bits && flags.HasFlag(value)) yield return value;
            }
        }
    }
}