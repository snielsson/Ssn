// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Specialized;
namespace Ssn.Utils.Extensions {
    public static class NameValueCollectionExtensions {
        public static string GetString(this NameValueCollection nameValueCollection, string key) {
            return nameValueCollection[key];
        }
        public static int GetInt(this NameValueCollection nameValueCollection, string key) {
            return int.Parse(nameValueCollection[key]);
        }
        public static long GetLong(this NameValueCollection nameValueCollection, string key) {
            return long.Parse(nameValueCollection[key]);
        }
        public static bool GetBool(this NameValueCollection nameValueCollection, string key) {
            return bool.Parse(nameValueCollection[key]);
        }
        public static T GetEnum<T>(this NameValueCollection nameValueCollection, string key) {
            return (T) Enum.Parse(typeof (T), nameValueCollection[key]);
        }
        public static object GetEnum(this NameValueCollection nameValueCollection, string key, Type type) {
            return Enum.Parse(type, nameValueCollection[key]);
        }
    }
}