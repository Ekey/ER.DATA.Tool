using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ER.Unpacker
{
    class IdxFileInfo
    {
        private static Dictionary<Int32, String> m_FolderList = new Dictionary<Int32, String>();
        private static List<String> m_FileList = new List<String>();

        public static void iLoadList(String m_ListFile)
        {
            m_FolderList.Clear();
            m_FileList.Clear();

            using (FileStream TListStream = File.OpenRead(m_ListFile))
            {
                Int32 dwTotalFolders = TListStream.ReadInt32();

                for (Int32 i = 0; i < dwTotalFolders; i++)
                {
                    Int32 dwFolderID = TListStream.ReadInt32();
                    Int32 bPathLength = TListStream.ReadByte();
                    String m_FolderName = Encoding.UTF8.GetString(TListStream.ReadBytes(bPathLength));

                    m_FolderList.Add(dwFolderID, m_FolderName);
                }

                Int32 dwTotalFiles = TListStream.ReadInt32();

                do
                {
                    Int32 dwFolderID = TListStream.ReadInt32();
                    Int32 bUnknown1 = TListStream.ReadByte();
                    Int32 bNameLength = TListStream.ReadByte();

                    Int32 bCheck = TListStream.ReadByte();
                    if (bCheck != 0x01)
                    {
                        TListStream.Position -= 1;
                    }

                    String m_FileName = Encoding.UTF8.GetString(TListStream.ReadBytes(bNameLength));
                    Int32 bHashLength = TListStream.ReadByte();
                    String m_FileHash = Encoding.UTF8.GetString(TListStream.ReadBytes(bHashLength));
                    Int32 dwSize = TListStream.ReadInt32();
                    Int32 dwUnknown1 = TListStream.ReadInt32();
                    Int32 bUnknown2 = TListStream.ReadByte();

                    String m_Folder = String.Empty;
                    if (m_FolderList.ContainsKey(dwFolderID))
                    {
                        m_FolderList.TryGetValue(dwFolderID, out m_Folder);
                    }

                    m_FileList.Add(m_Folder + "/" + m_FileName);
                }
                while (TListStream.Position != TListStream.Length);

                //StreamWriter TFileListWritter = new StreamWriter("data.list");
                //m_FileList.ForEach(TFileListWritter.WriteLine);
                //TFileListWritter.Close();

                //foreach (var m_File in m_FileList)
                //{
                //    Console.WriteLine(m_File.ToString());
                //}

                TListStream.Flush();
            }
        }
    }
}
