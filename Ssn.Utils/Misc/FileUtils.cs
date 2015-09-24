// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.IO;
namespace Ssn.Utils.Misc {
    public static class FileUtils {
        public static void CopyDir(string srcDir, string dstDir, bool recursive = true, bool overwrite = false) {
            Console.WriteLine("Copying " + srcDir + " to " + dstDir + (recursive ? " recursively." : "."));
            if (!Directory.Exists(srcDir)) throw new ArgumentException(srcDir + " does not exist");
            if (!Directory.Exists(dstDir)) Directory.CreateDirectory(dstDir);
            foreach (string file in Directory.EnumerateFiles(srcDir)) {
                File.Copy(file, Path.Combine(dstDir, Path.GetFileName(file)), overwrite);
            }
            if (recursive) {
                foreach (string dir in Directory.EnumerateDirectories(srcDir)) {
                    CopyDir(dir, Path.Combine(dstDir, Path.GetFileName(dir)), true, overwrite);
                }
            }
        }
    }
}