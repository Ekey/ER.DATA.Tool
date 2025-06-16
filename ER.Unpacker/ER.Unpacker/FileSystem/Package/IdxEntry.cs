using System;

namespace ER.Unpacker
{
    class IdxEntry
    {
        public String m_Hash { get; set; } // MD5 of file data with JNTE header
        public UInt32 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwDecompressedSize { get; set; }
    }
}
