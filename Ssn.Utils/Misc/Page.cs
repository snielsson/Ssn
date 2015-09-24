// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System.Collections.Generic;
namespace Ssn.Utils.Misc {
    public class Page<T> {
        public Page() {
            PageSize = 10;
            PageNumber = 1;
        }
        public long TotalCount { get; set; }
        private long _pageSize;
        public long PageSize { get { return _pageSize; } set { _pageSize = value < 1 ? 10 : value; } }

        private long _pageNumber;
        public long PageNumber { get { return _pageNumber; } set { _pageNumber = value < 1 ? 1 : value; } }
        public IEnumerable<T> Items { get; set; }
        public long Skip => PageSize*(PageNumber - 1);
        public long Take => PageSize;
    }
}