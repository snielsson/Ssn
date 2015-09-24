// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
using System.Collections.Generic;
using System.Linq;
using Ssn.Utils.Misc;
namespace Ssn.Utils.Extensions {
    public static class IEnumerableExtensions {
        public static int DefaultPageSize { get; set; } = 10;
        public static Page<T> Page<T>(this IEnumerable<T> @this, int page = 1, int? pageSize = null) {
            if (page <= 0) throw new ArgumentException("page <= 0");
            if (pageSize.GetValueOrDefault() < 0) throw new ArgumentException("pageSize < 0");
            var takeCount = pageSize ?? DefaultPageSize;
            var skipCount = takeCount*(page - 1);
            return new Page<T> {
                Items = @this.Skip(skipCount).Take(takeCount).ToList(),
                PageNumber = page,
                PageSize = takeCount,
                TotalCount = -1
            };
        }
    }
}