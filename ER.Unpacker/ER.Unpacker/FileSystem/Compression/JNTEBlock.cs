using System;

namespace ER.Unpacker
{
    class JNTEBlock
    {
        public CompressionType bCompressionType { get; set; }
        public Int32 dwCompressedSize { get; set; } // BigEndian
        public Int32 dwDecompressedSize { get; set; } // BigEndian
    }
}
