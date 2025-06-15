using System;

namespace ER.Unpacker
{
    class IdxEntry
    {
        public UInt32 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwDecompressedSize { get; set; }
    }
}
