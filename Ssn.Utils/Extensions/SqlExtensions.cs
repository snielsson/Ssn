// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Ssn.Utils.Misc;
namespace Ssn.Utils.Extensions {
    public static class SqlExtensions {
        private class SqlTypeInfo {
            public SqlTypeInfo(Type type) {
                Fields = Reflect.NamesOfPublicGetSetters(type);
                FieldsString = string.Join(",", Fields);
                FieldsAsParameters = Fields.Select(x => "@" + x).ToArray();
                FieldsAsParametersString = string.Join(",", FieldsAsParameters);
            }
            public IEnumerable<string> Fields { get; }
            public IEnumerable<string> FieldsAsParameters { get; }
            public string FieldsString { get; }
            public string FieldsAsParametersString { get; }
        }

        private static ConcurrentDictionary<Type, SqlTypeInfo> _cache => new ConcurrentDictionary<Type, SqlTypeInfo>();
        public static IEnumerable<string> SqlFields<T>() {
            return SqlFields(typeof (T));
        }
        public static IEnumerable<string> SqlFields(this object @this) {
            return SqlFields(@this.GetType());
        }
        public static string SqlFieldsString(this object @this) {
            return SqlFieldsString(@this.GetType());
        }
        public static IEnumerable<string> SqlFieldsAsParameters(this object @this) {
            return SqlFieldsAsParameters(@this.GetType());
        }
        public static string SqlFieldsAsParametersString(this object @this) {
            return SqlFieldsAsParametersString(@this.GetType());
        }
        public static IEnumerable<string> SqlFields(this Type @this) {
            return _cache.GetOrAdd(@this, _ => new SqlTypeInfo(@this)).Fields;
        }
        public static string SqlFieldsString(this Type @this) {
            return _cache.GetOrAdd(@this, _ => new SqlTypeInfo(@this)).FieldsString;
        }
        public static IEnumerable<string> SqlFieldsAsParameters(this Type @this) {
            return _cache.GetOrAdd(@this, _ => new SqlTypeInfo(@this)).FieldsAsParameters;
        }
        public static string SqlFieldsAsParametersString(this Type @this) {
            return _cache.GetOrAdd(@this, _ => new SqlTypeInfo(@this)).FieldsAsParametersString;
        }

        public static string SelectFields(this Type @this) {
            return "SELECT " + @this.SqlFields();
        }

    }
}