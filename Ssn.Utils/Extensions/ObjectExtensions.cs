// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using FastMember;
using Jil;
namespace Ssn.Utils.Extensions {
    /// <summary>
    /// Various general purpose extensions of the System.Object class, ie. it applies to all kinds of objects.
    /// </summary>
    public static class ObjectExtensions {
        private static readonly Options _jilToJsonOptions = new Options(false, false, false, DateTimeFormat.MillisecondsSinceUnixEpoch, true);
        private static readonly Options _jilCloneOptions = new Options(false, false, false, DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch, true);
        private static readonly Options _jilPrettyOptions = new Options(true, false, false, DateTimeFormat.MillisecondsSinceUnixEpoch, true);
        /// <summary>
        /// Deep clones an object by using the high performance JSON serializer JIL.
        /// </summary>
        /// <typeparam name="T">Type of the object to clone.</typeparam>
        /// <param name="obj">The object to clone.</param>
        /// <returns>A deep clone of the object argument.</returns>
        public static T Clone<T>(this T obj) {
            return JSON.Deserialize<T>(JSON.Serialize(obj, _jilCloneOptions));
        }

        /// <summary>
        /// Deep equality by comparing the JSON representation of two objects.
        /// </summary>
        /// <typeparam name="T">Type of the object to compare.</typeparam>
        /// <param name="obj">The object to compare.</param>
        /// <param name="other">The other object compare.</param>
        /// <returns>True if the objects JSON representations are equal.</returns>
        public static bool IsJsonEqualTo<T>(this T obj, T other) {
            if (ReferenceEquals(obj, other)) return true;
            if (obj.GetType() != other.GetType()) return false;
            return JSON.Serialize(obj, _jilCloneOptions) == JSON.Serialize(other, _jilCloneOptions);
        }

        /// <summary>
        /// Serializes an object to (formatted) JSON using the high performance JSON serializer JIL.
        /// </summary>
        /// <typeparam name="T">Type of the object being serialised.</typeparam>
        /// <param name="this">The object to serialize.</param>
        /// <returns>The object argument as a JSON string.</returns>
        public static string ToJson<T>(this T @this) {
            var result = JSON.Serialize(@this, _jilToJsonOptions);
            return result;
        }

        /// <summary>
        /// Serializes an object to pretty printet (formatted) JSON using the high performance JSON serializer JIL.
        /// </summary>
        /// <typeparam name="T">Type of the object being serialised.</typeparam>
        /// <param name="this">The object to serialize.</param>
        /// <returns>The object argument as a pretty printet (formattet) JSON string.</returns>
        public static string ToPrettyJson<T>(this T @this) {
            return JSON.Serialize(@this, _jilPrettyOptions);
        }

        /// <summary>
        /// Writes the argument as pretty printed JSON to the given TextWriter, which defaults to Console.Out.
        /// </summary>
        /// <typeparam name="T">Type of the arguments being serialized to JSON and dumped.</typeparam>
        /// <param name="this">The argument to dump as JSON.</param>
        /// <param name="writer">The TextWriter to write the JSON to, default to Console.Out.</param>
        public static void Dump<T>(this T @this, TextWriter writer = null) {
            writer = writer ?? Console.Out;
            writer.WriteLine(@this.ToPrettyJson());
        }

        public static bool Validate(this object @this, ICollection<ValidationResult> results = null, ValidationContext validationContext = null)
        {
            validationContext = validationContext ?? new ValidationContext(@this);
            if (results != null) return Validator.TryValidateObject(@this, validationContext, results, true);
            Validator.ValidateObject(@this, validationContext, true);
            return true;
        }

        public static bool HasValue<T>(this T @this) {
            return !EqualityComparer<T>.Default.Equals(@this, default(T));
        }

        public static TAttribute GetPropertyAttribute<TAttribute>(this object @this, string nameOfPropertyWithAttribute) where TAttribute : Attribute {
            var type = @this.GetType();
            var info = type.GetProperty(nameOfPropertyWithAttribute);
            var attribute = Attribute.GetCustomAttribute(info, typeof (TAttribute));
            return (TAttribute) attribute;
        }

        public static object Set<T>(this object @this, string propName, T value, bool allowNonPublicAccess = false)
        {
            ObjectAccessor.Create(@this, allowNonPublicAccess)[propName] = value;
            return @this;
        }
        public static T Get<T>(this object @this, string propName, bool allowNonPublicAccess = false)
        {
            return (T)ObjectAccessor.Create(@this, allowNonPublicAccess)[propName];
        }
    }
}