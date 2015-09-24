// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Generic;
using System.Text;
namespace Ssn.Utils.Extensions {
    public static class ExceptionExtensions {
        /// <summary>
        /// Convert an exception and the entire chains of inner exceprtions to a string.
        /// </summary>
        /// <param name="ex">The exception to convert to a string.</param>
        /// <returns>The string of all exceptions in the chaing of inner exceptions.</returns>
        public static string ToStringRecursive(this Exception ex) {
            var sb = new StringBuilder();
            var instances = new Dictionary<Exception, int>();
            sb.AppendLine(ex.ToString());
            return ToStringRecursiveCore(ex.InnerException, sb, instances);
        }

        private static string ToStringRecursiveCore(this Exception ex, StringBuilder sb, Dictionary<Exception, int> instances) {
            if (ex == null) return sb.ToString();
            if (instances.ContainsKey(ex)) {
                sb.AppendLine("Circular reference detected for exception:\n" + ex);
                return sb.ToString();
            }
            sb.AppendLine(ex.ToString());
            return ToStringRecursiveCore(ex.InnerException, sb, instances);
        }
    }
}