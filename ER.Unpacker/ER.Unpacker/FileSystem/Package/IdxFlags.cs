using System;

namespace ER.Unpacker
{
    [Flags]
    public enum CompressionType
    {
        None,
        LZ4,
        LZMA,
        ZSTD
    }
}
