// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Generic;
namespace Ssn.Utils.Misc {
    /// <summary>
    /// Implementation of IComparer{T} based on another one;
    /// this simply reverses the original comparison.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ReverseComparer<T> : IComparer<T> {
        private readonly IComparer<T> originalComparer;

        /// <summary>
        /// Returns the original comparer; this can be useful
        /// to avoid multiple reversals.
        /// </summary>
        public IComparer<T> OriginalComparer { get { return originalComparer; } }

        /// <summary>
        /// Creates a new reversing comparer.
        /// </summary>
        /// <param name="original">
        /// The original comparer to
        /// use for comparisons.
        /// </param>
        public ReverseComparer(IComparer<T> original) {
            if (original == null) throw new ArgumentNullException("original");
            originalComparer = original;
        }

        /// <summary>
        /// Returns the result of comparing the specified
        /// values using the original
        /// comparer, but reversing the order of comparison.
        /// </summary>
        public int Compare(T x, T y) {
            return originalComparer.Compare(y, x);
        }
    }
}