using System;
using System.IO;

namespace HasFile {

    public abstract class FileCheck {

        public abstract string Value { get; }

        public abstract void TransformBlock ( Stream stream, Func<int, bool> func );

        public static string GetHash ( string opt, Stream stream, Func<int, bool> func ) {
            FileCheck check;

            switch ( opt.ToLower() ) {
                case "crc32":
                    check = new Crc32();
                    break;

                case "md5":
                    check = new FileMD5();
                    break;

                default:
                    check = new FileSha1();
                    break;
            }

            check.TransformBlock(stream, func);

            return check.Value;
        }
    }
}