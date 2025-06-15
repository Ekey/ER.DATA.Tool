using System;

namespace ER.Unpacker
{
    class IdxHeader
    {
        public Int32 dwTotalContents { get; set; } // BigEndian
        public Int32 dwTotalFiles { get; set; } // BigEndian
    }
}
