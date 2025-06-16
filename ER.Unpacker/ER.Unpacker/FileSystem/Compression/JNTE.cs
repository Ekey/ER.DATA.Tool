using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Policy;
using System.Runtime.InteropServices;

namespace ER.Unpacker
{
    class JNTE
    {
        [DllImport("libjnte.dll", EntryPoint = "iJnteDecompress", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 iJnteDecompress(Byte[] lpSrcBuffer, Int32 dwCompressedSize, Byte[] lpDstBuffer, Int32 dwDecompressedSize, Int32 dwCompressionType);

        private static List<JNTEBlock> m_BlockTable = new List<JNTEBlock>();

        private static Byte[] iDecompressBlocks(Stream TStream)
        {
            Int32 dwOffset = 0;
            Int32 dwTotalSize = 0;
            Byte[] lpResult = new Byte[] { };

            for (Int32 i = 0; i < m_BlockTable.Count; i++)
            {
                Byte[] lpDecompressedBlock = new Byte[m_BlockTable[i].dwDecompressedSize];
                Byte[] lpCompressedBlock = TStream.ReadBytes(m_BlockTable[i].dwCompressedSize);

                dwTotalSize += m_BlockTable[i].dwDecompressedSize;
                Array.Resize(ref lpResult, dwTotalSize);

                if (m_BlockTable[i].bCompressionType == CompressionType.None)
                {
                    Array.Copy(lpCompressedBlock, 0, lpResult, dwOffset, m_BlockTable[i].dwCompressedSize);
                }
                else
                {
                    iJnteDecompress(lpCompressedBlock, m_BlockTable[i].dwCompressedSize, lpDecompressedBlock, m_BlockTable[i].dwDecompressedSize, (Int32)m_BlockTable[i].bCompressionType);
                    Array.Copy(lpDecompressedBlock, 0, lpResult, dwOffset, m_BlockTable[i].dwDecompressedSize);
                }

                dwOffset += m_BlockTable[i].dwDecompressedSize;
            }

            return lpResult;
        }

        public static Byte[] iDecompress(Byte[] lpBuffer, Int32 dwOffset = 0)
        {
            m_BlockTable.Clear();

            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer) { Position = dwOffset })
            {
                var m_Header = new JNTEHeader();

                m_Header.dwMagic = TMemoryStream.ReadUInt32();

                if (m_Header.dwMagic == 0x45544E4A) // JNTE
                {
                    m_Header.dwTotalBlocks = TMemoryStream.ReadInt32(true);

                    for (Int32 i = 0; i < m_Header.dwTotalBlocks; i++)
                    {
                        var m_Block = new JNTEBlock();

                        m_Block.bCompressionType = (CompressionType)TMemoryStream.ReadByte();
                        m_Block.dwDecompressedSize = TMemoryStream.ReadInt32(true);
                        m_Block.dwCompressedSize = TMemoryStream.ReadInt32(true);

                        m_BlockTable.Add(m_Block);
                    }

                    lpBuffer = iDecompressBlocks(TMemoryStream);

                    return lpBuffer;
                }

                TMemoryStream.Flush();

                return lpBuffer;
            }
        }
    }
}
