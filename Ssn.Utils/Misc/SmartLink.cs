// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
namespace Ssn.Utils.Misc {
    public class SmartLink {
        private const string _alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_";
        private static readonly int _alphabetLength = _alphabet.Length;
        private static readonly Random _random = new Random();
        public static string FromNumber(long val, int minLength = 0) {
            var result = new char[Math.Max(minLength, 1 + val/_alphabetLength)];
            for (int i = 0; i < result.Length; i++) {
                var index = (int) (val & 0x3f);
                result[i] = _alphabet[index];
                val >>= 6;
            }
            return new string(result);
        }
        //public static string FromString(string str, int minLength = 0) {
        //    for (var i = 0; i < str.Length; i+=2) {
        //        var index = (int) (val & 0x3f);
        //        result[i] = _alphabet[index]; 
        //        val >>= 6;
        //    }
        //    return new string(result);
        //} 
        public static string Random(int length) {
            var result = new char[length];
            for (int i = 0; i < result.Length; i++) {
                result[i] = _alphabet[_random.Next(0, _alphabetLength)];
            }
            return new string(result);
        }
    }
}