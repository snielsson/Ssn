// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.IO;
using System.Reflection;
namespace Ssn.Utils.Extensions {
    public static class AssemblyExtensions {
        public static string VersionString(this Assembly assembly) {
            return assembly.GetName().Version.ToString();
        }

        public static DateTime BuildTime(this Assembly assembly) {
            return LinkerTimestamp(assembly);
        }

        public static string GetAssemblyInfo(this Assembly assembly) {
            return string.Format("Assembly: {0}, version= {1}, buildtime= {2:yyyy-MM-dd hh:mm:ss} UTC", assembly.GetName().Name, assembly.GetName().Version, LinkerTimestamp(assembly));
        }

        private static DateTime LinkerTimestamp(this Assembly assembly) {
            assembly = assembly ?? Assembly.GetCallingAssembly();
            string filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            var b = new byte[2048];
            Stream s = null;
            try {
                s = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally {
                if (s != null) s.Close();
            }
            int i = BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(secondsSince1970);
            return dt;
        }
    }
}