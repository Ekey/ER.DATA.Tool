using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;

namespace ER.Unpacker
{
    class IdxUnpack
    {
        private static Dictionary<String, IdxEntry> m_EntryTable = new Dictionary<String, IdxEntry>();

        private static IdxEntry iGetEntryRecordByHash(String m_Hash)
        {
            IdxEntry m_Entry = null;

            if (m_EntryTable.ContainsKey(m_Hash))
            {
                m_EntryTable.TryGetValue(m_Hash, out m_Entry);
            }

            return m_Entry;
        }

        public static void iDoIt(String m_IndexFile, String m_DstFolder)
        {
            //String m_FileInfo = m_IndexFile.Replace(".idx", ".files_info");
            
            //if (File.Exists(m_FileInfo))
            //{
            //    IdxFileInfo.iLoadList(m_FileInfo);
            //}

            IdxList.iLoadProject();

            using (FileStream TIndexStream = File.OpenRead(m_IndexFile))
            {
                var m_Header = new IdxHeader();

                m_Header.dwTotalContents = TIndexStream.ReadInt32(true);
                m_Header.dwTotalFiles = TIndexStream.ReadInt32(true);

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    var m_Entry = new IdxEntry();

                    m_Entry.m_Hash = Utils.iGetStringFromBytes(TIndexStream.ReadBytes(16));
                    m_Entry.dwOffset = TIndexStream.ReadUInt32(true);
                    m_Entry.dwCompressedSize = TIndexStream.ReadInt32(true);
                    m_Entry.dwDecompressedSize = TIndexStream.ReadInt32(true);

                    m_EntryTable.Add(m_Entry.m_Hash, m_Entry);
                }

                String m_DataFile = Path.GetDirectoryName(m_IndexFile) + @"\" + Path.GetFileNameWithoutExtension(m_IndexFile) + ".data";

                if (!File.Exists(m_DataFile))
                {
                    throw new Exception("[ERROR]: Unable to open data file -> " + m_DataFile + " <- file does not exist!");
                }

                using (FileStream TDataStream = File.OpenRead(m_DataFile))
                {
                    for (Int32 i = 0; i < m_Header.dwTotalContents; i++)
                    {
                        UInt64 dwHashName = TIndexStream.ReadUInt64(true);
                        String m_Hash = Utils.iGetStringFromBytes(TIndexStream.ReadBytes(16));

                        var m_Entry = iGetEntryRecordByHash(m_Hash);

                        if (m_Entry != null)
                        {
                            String m_FileName = IdxList.iGetNameFromHashList(dwHashName);
                            String m_FullPath = m_DstFolder + m_FileName;

                            Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                            Utils.iCreateDirectory(m_FullPath);

                            TDataStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                            var lpBuffer = TDataStream.ReadBytes(m_Entry.dwCompressedSize);

                            lpBuffer = JNTE.iDecompress(lpBuffer);

                            File.WriteAllBytes(m_FullPath, lpBuffer);
                        }
                        else
                        {
                            Utils.iSetWarning("[WARNING]: Entry with data hash " + m_Hash + " not found and was skipped!");
                        }
                    }

                    TIndexStream.Flush();
                    TDataStream.Flush();
                }
            }
        }
    }
}
