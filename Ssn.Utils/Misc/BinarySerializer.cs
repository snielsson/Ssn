// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
namespace Ssn.Utils.Misc {
    public interface IBinarySerializable {
        byte[] Serialize();
        void Deserialize(byte[] bytes, int offset);
    }

    public static class BinarySerializer {
        public static Stream CreateZipFileStream(string filePath, int compressionLevel = 9) {
            var zipOutputStream = new ZipOutputStream(File.OpenWrite(filePath));
            zipOutputStream.SetLevel(compressionLevel);
            var fileName = Path.GetFileName(filePath);
            if (fileName.EndsWith("zip")) fileName = Path.GetFileNameWithoutExtension(fileName);
            zipOutputStream.PutNextEntry(new ZipEntry(fileName) {
                DateTime = DateTime.Now
            });
            return zipOutputStream;
        }

        public static void Write(Stream stream, IBinarySerializable item) {
            var bytes = item.Serialize();
            stream.Write(bytes, 0, bytes.Length);
        }

        public static IEnumerable<T> LoadFile<T>(string filePath, int recordSize) where T : IBinarySerializable, new() {
            using (var inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                using (var zipInputStream = new ZipInputStream(inputFileStream)) {
                    var inZipEntry = zipInputStream.GetNextEntry();
                    inputFileStream.Seek(0, SeekOrigin.Begin);
                    var zipFile = new ZipFile(inputFileStream);
                    using (var stream = zipFile.GetInputStream(inZipEntry)) {
                        var bufSize = recordSize*512;
                        var buffer = new byte[bufSize];
                        int bytesRead;
                        do {
                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                            for (var i = 0; i < bytesRead; i += recordSize) {
                                var result = new T();
                                result.Deserialize(buffer, i);
                                yield return result;
                            }
                        }
                        while (bytesRead == bufSize);
                    }
                }
            }
        }

        public static IEnumerable<string> LoadZippedTextFileLines(string filePath) {
            using (var inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                using (var zipInputStream = new ZipInputStream(inputFileStream)) {
                    var inZipEntry = zipInputStream.GetNextEntry();
                    inputFileStream.Seek(0, SeekOrigin.Begin);
                    var zipFile = new ZipFile(inputFileStream);
                    using (var reader = new StreamReader(zipFile.GetInputStream(inZipEntry))) {
                        string line;
                        do {
                            line = reader.ReadLine();
                            if (line != null) yield return line;
                        }
                        while (line != null);
                    }
                }
            }
        }
    }
}