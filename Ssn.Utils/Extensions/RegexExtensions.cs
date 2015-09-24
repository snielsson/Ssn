// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Ssn.Utils.Extensions {
    public static class RegexExtensions {
        private static readonly Lazy<ConcurrentDictionary<string, Regex>> _regexCache = new Lazy<ConcurrentDictionary<string, Regex>>(() => new ConcurrentDictionary<string, Regex>());
        /// <summary>
        /// The RegexOptions to se when auto creating new regex objects from the regex related string extension methods. Default value is RegexOptions.Compiled.
        /// </summary>
        public static RegexOptions DefaultRegexOptions = RegexOptions.Compiled;
        private static Regex GetCachedRegex(string pattern, RegexOptions regexOptions) {
            var key = pattern + (int) regexOptions;
            return _regexCache.Value.GetOrAdd(key, _ => new Regex(pattern, regexOptions));
        }

        public static string Extract(this string @this, string pattern, RegexOptions? options = null)
        {
            var match = GetCachedRegex(pattern, options ?? DefaultRegexOptions).Match(@this);
            if (!match.Success) return null;
            return match.Groups[1].Value;
        }

        /// <summary>
        /// Extracts an IEnumerable of all matches of a single match group int the pattern regex. Default match group index is 1.  Optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to match against pattern</param>
        /// <param name="pattern">The pattern to construct a regex from. The Regex will be cached for future matches.</param>
        /// <param name="group">The index of the group match to extract. </param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>An IEnumerable of all matches of the given match group.</returns>
        public static IEnumerable<string> ExtractSingleGroup(this string @this, string pattern, int group = 1, RegexOptions? options = null) {
            var matches = GetCachedRegex(pattern, options ?? DefaultRegexOptions).Matches(@this);
            if (matches.Count == 0) yield break;
            for (var i = 0; i < matches.Count; i++) {
                var match = matches[i];
                yield return match.Groups[group].Value;
            }
        }

        /// <summary>
        /// Extracts an IEnumerable arrays of matches from all match groups in the pattern regex. Optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to match against pattern.</param>
        /// <param name="pattern">The pattern to construct a regex from. The Regex will be cached for future matches.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>An IEnumerable of array of matches of all matches groups.</returns>
        public static IEnumerable<string[]> ExtractAllGroups(this string @this, string pattern, RegexOptions? options = null) {
            var matches = GetCachedRegex(pattern, options ?? DefaultRegexOptions).Matches(@this);
            if (matches.Count == 0) yield break;
            for (var i = 0; i < matches.Count; i++) {
                var match = matches[i];
                var values = new string[match.Groups.Count];
                for (var j = 0; j < match.Groups.Count; j++) {
                    values[j] = match.Groups[j].Value;
                }
                yield return values;
            }
        }

        /// <summary>
        /// Shorthand helper to make pattern based string replacement, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to make replacements in. This string will not change, but a modified copied will be returned.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="replacement">The string to replace all matches with.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>A modified copy of input string with all matches replaced with replacement string.</returns>
        public static string Replace(this string @this, string pattern, string replacement, RegexOptions? options = null) {
            return GetCachedRegex(pattern, options ?? DefaultRegexOptions).Replace(@this, replacement);
        }

        /// <summary>
        /// Shorthand helper to make pattern based string splitting, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to split.</param>
        /// <param name="pattern">The pattern to split on.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <param name="count">Optional count of elements to include in result.</param>
        /// <param name="startat"></param>
        /// <returns>Array of strings with the results of splitting the input string on the pattern.</returns>
        public static string[] Split(this string @this, string pattern, RegexOptions? options = null, int count = 0, int startat = 0) {
            return GetCachedRegex(pattern, options ?? DefaultRegexOptions).Split(@this, count, startat);
        }

        /// <summary>
        /// Tests if string matches a pattern, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to test.</param>
        /// <param name="pattern">The pattern to use in test.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>True if string matches, false otherwise.</returns>
        public static bool Matches(this string @this, string pattern, RegexOptions? options = null) {
            var regex = GetCachedRegex(pattern, options ?? DefaultRegexOptions);
            return regex.IsMatch(@this);
        }

        /// <summary>
        /// Get the match result of mathcing string against pattern, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to match.</param>
        /// <param name="pattern">The pattern to match against.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>The Match object result from the RegexMatch.</returns>
        public static Match Match(this string str, string pattern, RegexOptions? options = null) {
            var regex = GetCachedRegex(pattern, options ?? DefaultRegexOptions);
            return regex.Match(str);
        }

        /// <summary>
        /// Get the match result of mathcing string against pattern, optimized to use an internal regex cache, shared by all regex related string extension methods of this library.
        /// </summary>
        /// <param name="this">The string to match.</param>
        /// <param name="pattern">The pattern to match against.</param>
        /// <param name="match">*Out parameter to hold the match result.</param>
        /// <param name="options">Optional regex options to use when constructing the regex from the pattern.</param>
        /// <returns>True if match succeeded, false otherwise. The match out parameter contains the resulting Match object or null.</returns>
        public static bool TryMatch(this string @this, string pattern, out Match match, RegexOptions? options = null) {
            var regex = GetCachedRegex(pattern, options ?? DefaultRegexOptions);
            match = regex.Match(@this);
            return match.Success;
        }

        /// <summary>
        /// Convenience method to replace in string based on regex. Does not use the regex cache, because the regex is provided as parameter.
        /// </summary>
        /// <param name="this">The string to replace in.</param>
        /// <param name="regex">The regex used to make the replacements.</param>
        /// <param name="replacement">The string to replace with.</param>
        /// <returns>Copy of input string with replacements made.</returns>
        public static string Replace(this string @this, Regex regex, string replacement) {
            return regex.Replace(@this, replacement);
        }

        /// <summary>
        /// Convenience method to split string on regex. Does not use the regex cache, because the regex is provided as parameter.
        /// </summary>
        /// <param name="this">The string to split.</param>
        /// <param name="regex">The regex used to perfortm the split.</param>
        /// <param name="count">Optional count argument for the Regex.Split method.</param>
        /// <param name="startAt">Optional startAt argument for the Regex.Splt method.</param>
        /// <returns>Array of strings with the results of splitting the input string on the regex.</returns>
        public static string[] Split(this string @this, Regex regex, int count = 0, int startAt = 0) {
            return regex.Split(@this, count, startAt);
        }
    }
}