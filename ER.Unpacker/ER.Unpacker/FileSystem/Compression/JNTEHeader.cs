using System;

namespace ER.Unpacker
{
    class JNTEHeader
    {
        public UInt32 dwMagic { get; set; } // JNTE
        public Int32 dwTotalBlocks { get; set; }
    }
}
