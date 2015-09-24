// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
using System.Collections.Generic;
namespace Ssn.Utils.Extensions {
    public static class TypeExtensions {
        public static object DefaultValue(this object obj) {
            return DefaultValue(obj.GetType());
        }
        public static object DefaultValue(this Type type) {
            if (!type.IsValueType) return null;
            return Activator.CreateInstance(type);
        }
        public static bool IsDefaultValue(this object obj) {
            if (obj == null) return true;
            return obj.Equals(DefaultValue(obj));
        }

        public static List<AttributeType> GetAttributes<AttributeType>(this Type type) where AttributeType : class {
            var result = new List<AttributeType>();
            foreach (Attribute attribute in Attribute.GetCustomAttributes(type)) {
                var typedAttribute = attribute as AttributeType;
                if (typedAttribute != null) result.Add(typedAttribute);
            }
            return result;
        }
    }
}