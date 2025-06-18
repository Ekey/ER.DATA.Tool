using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ER.Unpacker
{
    class JNTE
    {
        //Thanks to cdj88888 - https://github.com/cdj88888/Studio/

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(String m_Library);

        private static List<JNTEBlock> m_BlocksTable = new List<JNTEBlock>();

        private delegate Int64 JNTE_LZ4Decompress(Byte[] lpSrcBuffer, Byte[] lpDstBuffer, Int32 dwCompressedSize, Int32 dwDecompressedSize);
        private delegate Int64 JNTE_ZSTDDecompress(Byte[] lpDstBuffer, Int32 dwDecompressedSize, Byte[] lpSrcBuffer, Int32 dwCompressedSize);

        private static JNTE_LZ4Decompress _LZ4Decompress;
        private static JNTE_ZSTDDecompress _ZSTDDecompress;

        public static Boolean iLoadUnityLibrary()
        {
            var m_Module = LoadLibrary("UnityPlayer.dll");

            if (m_Module != null)
            {
                _LZ4Decompress = Marshal.GetDelegateForFunctionPointer<JNTE_LZ4Decompress>((IntPtr)(m_Module + 0x108D160));
                _ZSTDDecompress = Marshal.GetDelegateForFunctionPointer<JNTE_ZSTDDecompress>((IntPtr)(m_Module + 0x10972F0));

                return true;
            }

            return false;
        }

        private static Byte[] iDecompressBlocks(Stream TStream)
        {
            Int32 dwOffset = 0;
            Int32 dwTotalSize = 0;
            Byte[] lpResult = new Byte[] { };

            for (Int32 i = 0; i < m_BlocksTable.Count; i++)
            {
                Byte[] lpDecompressedBlock = new Byte[m_BlocksTable[i].dwDecompressedSize];
                Byte[] lpCompressedBlock = TStream.ReadBytes(m_BlocksTable[i].dwCompressedSize);

                dwTotalSize += m_BlocksTable[i].dwDecompressedSize;
                Array.Resize(ref lpResult, dwTotalSize);

                if (m_BlocksTable[i].bCompressionType == CompressionType.None)
                {
                    Array.Copy(lpCompressedBlock, 0, lpResult, dwOffset, m_BlocksTable[i].dwCompressedSize);
                    dwOffset += m_BlocksTable[i].dwCompressedSize;
                }
                else
                {
                    switch(m_BlocksTable[i].bCompressionType)
                    {
                        case CompressionType.LZ4: _LZ4Decompress(lpCompressedBlock, lpDecompressedBlock, m_BlocksTable[i].dwCompressedSize, m_BlocksTable[i].dwDecompressedSize); break;
                        case CompressionType.ZSTD: _ZSTDDecompress(lpDecompressedBlock, m_BlocksTable[i].dwDecompressedSize, lpCompressedBlock, m_BlocksTable[i].dwCompressedSize); break;
                    }

                    Array.Copy(lpDecompressedBlock, 0, lpResult, dwOffset, m_BlocksTable[i].dwDecompressedSize);
                    dwOffset += m_BlocksTable[i].dwDecompressedSize;
                }
            }

            return lpResult;
        }

        public static Byte[] iDecompress(Byte[] lpBuffer, Int32 dwOffset = 0)
        {
            m_BlocksTable.Clear();

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

                        m_BlocksTable.Add(m_Block);
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
