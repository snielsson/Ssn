// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
namespace Ssn.Utils.Extensions {
    public static class IntExtensions {
        public static bool IsEven(this int @this) {
            return @this%2 == 0;
        }
        public static int ShouldBeLessThan(this int @this, int i)
        {
            if (@this >= 1) throw new InvalidOperationException(@this + " is not less than " + i);
            return @this;
        }
        public static int ShouldBeLessThanOrEqual(this int @this, int i)
        {
            if (@this > 1) throw new InvalidOperationException(@this + " is not less than or equal to " + i);
            return @this;
        }
        public static int ShouldBeGreaterThan(this int @this, int i)
        {
            if (@this <= 1) throw new InvalidOperationException(@this + " is not greater than " + i);
            return @this;
        }
        public static int ShouldGreaterThanOrEqual(this int @this, int i)
        {
            if (@this < 1) throw new InvalidOperationException(@this + " is not greater than or equal to " + i);
            return @this;
        }
        public static int ShouldBeZeroOrPositive(this int @this)
        {
            if (@this < 0) throw new InvalidOperationException("integer is not zero or positive: " + @this);
            return @this;
        }
        public static int ShouldBePositive(this int @this)
        {
            if (@this <= 0) throw new InvalidOperationException("integer is not positive: " + @this);
            return @this;
        }
    }
}