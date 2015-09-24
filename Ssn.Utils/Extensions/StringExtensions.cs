// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
namespace Ssn.Utils.Extensions {
    public static class StringExtensions {
        public static int ToInt(this string @this) {
            return int.Parse(@this);
        }

        /// <summary>
        /// Encode string to Base64.
        /// </summary>
        /// <param name="this">The string to encode.</param>
        /// <returns>String encoded to Base64.</returns>
        public static string Base64Encode(this string @this) {
            var plainTextBytes = Encoding.UTF8.GetBytes(@this);
            return Convert.ToBase64String(plainTextBytes);
        }
        /// <summary>
        /// Decode Base64 string.
        /// </summary>
        /// <param name="this">The Base64 string to decode.</param>
        /// <returns>The decoded string value.</returns>
        public static string Base64Decode(this string @this) {
            var base64EncodedBytes = Convert.FromBase64String(@this);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        /// <summary>
        /// Converts a byte array to a string of hex chars.
        /// </summary>
        /// <param name="bytes">The bytes to represent as a string.</param>
        /// <returns>The byte array as a string of hex chars.</returns>
        public static string ToHexString(this byte[] bytes) {
            var c = new char[bytes.Length*2];
            for (var i = 0; i < bytes.Length; ++i) {
                var b = ((byte) (bytes[i] >> 4));
                c[i*2] = (char) (b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte) (bytes[i] & 0xF));
                c[i*2 + 1] = (char) (b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }
        /// <summary>
        /// Converts a string of hex chars to a byte array. Is not robust against bad input, so only pass in strings with an event number of chars in the rnage [0123456789ABCDEF].
        /// </summary>
        /// <param name="hexString">The hexstring to convert to a byte array.</param>
        /// <returns>The byte array of the hexstring.</returns>
        public static byte[] HexStringToByteArray(this string hexString) {
            var hexStringLength = hexString.Length;
            if (hexStringLength%2 == 1) throw new ArgumentException("hexString is not valid. Must contain an even number of chars in the range [0123456789ABCDEF].");
            var b = new byte[hexStringLength/2];
            for (var i = 0; i < hexStringLength; i += 2) {
                var topChar = (hexString[i] > 0x40 ? hexString[i] - 0x37 : hexString[i] - 0x30) << 4;
                var bottomChar = hexString[i + 1] > 0x40 ? hexString[i + 1] - 0x37 : hexString[i + 1] - 0x30;
                b[i/2] = Convert.ToByte(topChar + bottomChar);
            }
            return b;
        }

        public static string Diff(this string @this, string str) {
            var sb = new StringBuilder();
            var lines1 = @this.Split(Environment.NewLine);
            var lines2 = str.Split(Environment.NewLine);
            for (var i = 0; i < Math.Min(lines1.Length, lines2.Length); i++) {
                var l1 = lines1[i];
                var l2 = lines2[i];
                if (l1 != l2) {
                    var limit = Math.Min(l1.Length, l2.Length);
                    for (var j = 0; j < limit; j++) {
                        if (l1[j] != l2[j] || j == limit - 1) {
                            if (i > 1) sb.AppendFormat("{0:D4}: ", i - 1).AppendLine(lines1[i - 2]);
                            if (i > 0) sb.AppendFormat("{0:D4}: ", i).AppendLine(lines1[i - 1]);
                            sb.AppendFormat("{0:D4}: ", i + 1).AppendLine(lines1[i]);
                            sb.Append(' ', 6 + j).AppendLine("^");
                            if (lines1.Length > i + 1) sb.AppendFormat("{0:D4}: ", i + 2).AppendLine(lines1[i + 1]);
                            if (lines1.Length > i + 2) sb.AppendFormat("{0:D4}: ", i + 3).AppendLine(lines1[i + 2]);

                            sb.AppendLine(new string('-', 6 + l1.Length));

                            if (i > 1) sb.AppendFormat("{0:D4}: ", i - 1).AppendLine(lines2[i - 2]);
                            if (i > 0) sb.AppendFormat("{0:D4}: ", i).AppendLine(lines2[i - 1]);
                            sb.AppendFormat("{0:D4}: ", i + 1).AppendLine(lines2[i]);
                            sb.Append(' ', 6 + j).AppendLine("^");
                            if (lines2.Length > i + 1) sb.AppendFormat("{0:D4}: ", i + 2).AppendLine(lines2[i + 1]);
                            if (lines2.Length > i + 2) sb.AppendFormat("{0:D4}: ", i + 3).AppendLine(lines2[i + 2]);
                            break;
                        }
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a slug.
        /// References:
        /// http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls
        /// http://www.unicode.org/reports/tr15/tr15-34.html
        /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// http://stackoverflow.com/questions/25259/how-do-you-include-a-webpage-title-as-part-of-a-webpage-url/25486#25486
        /// http://stackoverflow.com/questions/3769457/how-can-i-remove-accents-on-a-string
        /// </summary>
        public static string ToSlug(this string @this, bool toLower = false) {
            if (@this == null)
                return "";
            var normalised = @this.Normalize(NormalizationForm.FormKD);
            const int maxlen = 80;
            var len = normalised.Length;
            var prevDash = false;
            var sb = new StringBuilder(len);
            char c;
            for (var i = 0; i < len; i++) {
                c = normalised[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')) {
                    if (prevDash) {
                        sb.Append('-');
                        prevDash = false;
                    }
                    sb.Append(c);
                }
                else if (c >= 'A' && c <= 'Z') {
                    if (prevDash) {
                        sb.Append('-');
                        prevDash = false;
                    }
                    // Tricky way to convert to lowercase
                    if (toLower)
                        sb.Append((char) (c | 32));
                    else
                        sb.Append(c);
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=') {
                    if (!prevDash && sb.Length > 0) prevDash = true;
                }
                else {
                    var swap = ConvertEdgeCases(c, toLower);
                    if (swap != null) {
                        if (prevDash) {
                            sb.Append('-');
                            prevDash = false;
                        }
                        sb.Append(swap);
                    }
                }
                if (sb.Length == maxlen)
                    break;
            }
            return sb.ToString();
        }

        private static string ConvertEdgeCases(char c, bool toLower) {
            string swap = null;
            switch (c) {
                case 'ı':
                    swap = "i";
                    break;
                case 'ł':
                    swap = "l";
                    break;
                case 'Ł':
                    swap = toLower ? "l" : "L";
                    break;
                case 'đ':
                    swap = "d";
                    break;
                case 'ß':
                    swap = "ss";
                    break;
                case 'Ø':
                    swap = toLower ? "oe" : "Oe";
                    break;
                case 'ø':
                    swap = "oe";
                    break;
                case 'Æ':
                    swap = toLower ? "ae" : "Ae";
                    break;
                case 'æ':
                    swap = "ae";
                    break;
                case 'Å':
                    swap = toLower ? "aa" : "Aa";
                    break;
                case 'å':
                    swap = "aa";
                    break;
                case 'Þ':
                    swap = "th";
                    break;
            }
            return swap;
        }

        public static string RemoveWhitespace(this string input) {
            int j = 0, inputlen = input.Length;
            var newarr = new char[inputlen];
            for (var i = 0; i < inputlen; ++i) {
                var tmp = input[i];
                if (char.IsWhiteSpace(tmp)) continue;
                newarr[j] = tmp;
                ++j;
            }
            return new string(newarr, 0, j);
        }

        private static readonly char[] _defaultTrimeChars = {' '};
        public static string TrimOrDefault(this string @this, string nullString, params char[] trimChars) {
            trimChars = trimChars ?? _defaultTrimeChars;
            if (string.IsNullOrEmpty(@this)) return nullString;
            return @this.Trim(trimChars);
        }

        private static readonly MD5 _md5 = MD5.Create();
        public static string ToMD5Hash(this string @this) {
            lock (_md5) {
                var bytes = _md5.ComputeHash(Encoding.UTF8.GetBytes(@this));
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
        }

        public static byte[] GetBytes(this string @this, Encoding encoding = null) {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetBytes(@this);
        }

        public static string Fmt(this string @this, params object[] args) {
            return string.Format(@this, args);
        }
        public static bool EqualTo(this string @this, string str, bool ignoreCase = false)
        {
            return string.Compare(@this, str, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) == 0;
        }

        public static string UcFirst(this string @this)
        {
            return @this[0].ToString().ToUpper() + @this.Substring(1);
        }

        public static string Zip(this string @this) {
            var bytes = Encoding.UTF8.GetBytes(@this);
            using (var msi = new MemoryStream(bytes)) {
                using (var mso = new MemoryStream()) {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                        msi.CopyTo(gs);
                    }
                    return Convert.ToBase64String(mso.ToArray());
                }
            }
        }

        public static string Unzip(this string @this) {
            var bytes = Convert.FromBase64String(@this);
            using (var msi = new MemoryStream(bytes)) {
                using (var mso = new MemoryStream()) {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                        gs.CopyTo(mso);
                    }
                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
        }

        public static bool LessThan(this string @this, string other, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase) {
            return string.Compare(@this, other, comparison) < 0;
        }
        public static bool LessThanEqualTo(this string @this, string other, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase) {
            return string.Compare(@this, other, comparison) <= 0;
        }
        public static bool GreaterThan(this string @this, string other, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase) {
            return string.Compare(@this, other, comparison) > 0;
        }
        public static bool GreaterThanEqualTo(this string @this, string other, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase) {
            return string.Compare(@this, other, comparison) >= 0;
        }
        public static bool EqualTo(this string @this, string other, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase) {
            return string.Compare(@this, other, comparison) == 0;
        }

        private static readonly byte[] _defaultEncryptionKey = new Guid("bc9b9f29-d9eb-425f-b8fd-4bfc447575a9").ToByteArray();
        private static readonly byte[] _defaultEncryptionVector = new Guid("5E86F633-5AB4-436A-B5C5-CEC59BBF636B").ToByteArray();
        private static Lazy<ICryptoTransform> _defaultEncryptor = new Lazy<ICryptoTransform>(() => new RijndaelManaged().CreateEncryptor(_defaultEncryptionKey, _defaultEncryptionVector));
        private static Lazy<ICryptoTransform> _defaultDecryptor = new Lazy<ICryptoTransform>(() => new RijndaelManaged().CreateDecryptor(_defaultEncryptionKey, _defaultEncryptionVector));
        private static readonly ReaderWriterLockSlim _encryptionReaderWriterLock = new ReaderWriterLockSlim();
        public static void InitEncryptor(byte[] key, byte[] initVector) {
            using (_encryptionReaderWriterLock.WriteLock()) {
                _defaultEncryptor = new Lazy<ICryptoTransform>(() => new RijndaelManaged().CreateEncryptor(_defaultEncryptionKey, _defaultEncryptionVector));
                _defaultDecryptor = new Lazy<ICryptoTransform>(() => new RijndaelManaged().CreateDecryptor(_defaultEncryptionKey, _defaultEncryptionVector));
            }
        }

        public static string Encrypt(this string text, byte[] key, byte[] vector = null) {
            using (_encryptionReaderWriterLock.ReadLock()) {
                var encryptor = new RijndaelManaged().CreateEncryptor(key, vector ?? _defaultEncryptionVector);
                return Encrypt(text, encryptor);
            }
        }

        public static string Encrypt(this string text, ICryptoTransform encryptor = null) {
            using (_encryptionReaderWriterLock.ReadLock()) {
                {
                    var inputbuffer = Encoding.Unicode.GetBytes(text);
                    var outputBuffer = (encryptor ?? _defaultEncryptor.Value).TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                    return Convert.ToBase64String(outputBuffer);
                }
            }
        }

        public static string Decrypt(this string text, byte[] key, byte[] vector = null) {
            using (_encryptionReaderWriterLock.ReadLock()) {
                {
                    var decryptor = new RijndaelManaged().CreateDecryptor(key, vector ?? _defaultEncryptionVector);
                    return Decrypt(text, decryptor);
                }
            }
        }
        public static string Decrypt(this string text, ICryptoTransform decryptor = null) {
            using (_encryptionReaderWriterLock.ReadLock()) {
                {
                    var inputbuffer = Convert.FromBase64String(text);
                    var outputBuffer = (decryptor ?? _defaultDecryptor.Value).TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                    return Encoding.Unicode.GetString(outputBuffer);
                }
            }
        }
    }
}