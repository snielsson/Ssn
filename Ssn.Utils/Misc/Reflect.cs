// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
namespace Ssn.Utils.Misc {
    public static class Reflect {
        public static IEnumerable<Type> GetImplementingTypes<T>(Assembly assembly = null) {
            Type type = typeof (T);
            Debug.Assert(type.IsInterface);
            assembly = assembly ?? Assembly.GetAssembly(type);
            IEnumerable<Type> result = assembly.GetTypes().Where(t => !t.IsInterface && t.GetInterfaces().Contains(type));
            return result;
        }

        public static IEnumerable<string> NamesOfPublicGetSetters<T>(Func<Type, Boolean> filter = null)
        {
            return NamesOfPublicGetSetters(typeof(T),filter);
        }
        public static string[] NamesOfPublicGetSetters(Type type, Func<Type,Boolean> filter = null) {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty)
                .Where(x => x.GetGetMethod() != null && x.GetSetMethod() != null && (filter == null || filter(type)) )
                .Select(x => x.Name)
                .ToArray();
        }

    }
}