using System;
using System.IO;
using System.Security.Cryptography;

namespace HasFile
{
    public class FileSha1 : FileCheck
    {
        private HashAlgorithm hash = SHA1.Create();

        public override string Value {
            get {
                return BitConverter.ToString(hash.Hash).Replace("-", string.Empty);
            }
        }

        public override void TransformBlock(Stream stream, Func<int, bool> numberCompleted) {
            stream.Position = 0;

            hash.Initialize();

            int progress = 10;

            long offset = 0;

            int bufferSize = 1048576; // 缓冲区大小，1MB

            long len = stream.Length;

            byte[] buff = new byte[bufferSize];

            while (offset < len) {
                long readSize = bufferSize;

                if (offset + readSize > len) {
                    readSize = len - offset;
                }

                stream.Read(buff, 0, Convert.ToInt32(readSize)); // 读取一段数据到缓冲区

                if (offset + readSize < len) {
                    hash.TransformBlock(buff, 0, Convert.ToInt32(readSize), buff, 0);
                } else {
                    hash.TransformFinalBlock(buff, 0, Convert.ToInt32(readSize));
                }

                if ((double)offset / (double)len * 100d > progress) {
                    progress += 10;

                    if (numberCompleted(progress) == false) {
                        Array.Clear(buff, 0, bufferSize);
                        buff = null;
                        return;
                    }
                }

                offset += bufferSize;
            }

            Array.Clear(buff, 0, bufferSize);
            buff = null;
        }
    }
}