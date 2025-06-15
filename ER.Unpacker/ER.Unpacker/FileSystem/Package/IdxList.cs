using System;
using System.Collections.Generic;
using System.IO;

namespace ER.Unpacker
{
    class IdxList
    {
        private static String m_Path = Utils.iGetApplicationPath() + @"\Projects\";
        private static String m_ProjectFile = "FileNames.list";
        private static String m_ProjectFilePath = m_Path + m_ProjectFile;
        private static Dictionary<UInt64, String> m_HashList = new Dictionary<UInt64, String>();

        public static void iLoadProject()
        {
            String m_Line = null;
            if (!File.Exists(m_ProjectFilePath))
            {
                Utils.iSetError("[ERROR]: Unable to load project file " + m_ProjectFile);
                return;
            }

            Int32 i = 0;
            m_HashList.Clear();

            StreamReader TProjectFile = new StreamReader(m_ProjectFilePath);
            while ((m_Line = TProjectFile.ReadLine()) != null)
            {
                UInt64 dwHash = MD5.iGetHash(m_Line.ToLower());

                if (m_HashList.ContainsKey(dwHash))
                {
                    String m_Collision = null;
                    m_HashList.TryGetValue(dwHash, out m_Collision);

                    Utils.iSetError("[ERROR]: [COLLISION]: at line " + i.ToString() + " " + m_Collision + " <-> " + " " + m_Line);
                    break;
                }

                m_HashList.Add(dwHash, m_Line.Replace("/", @"\"));
                i++;
            }

            TProjectFile.Close();
            Console.WriteLine("[INFO]: Project File Loaded: {0}", i);
            Console.WriteLine();
        }

        public static String iGetNameFromHashList(UInt64 dwHash)
        {
            String m_FileName = null;

            if (m_HashList.ContainsKey(dwHash))
            {
                m_HashList.TryGetValue(dwHash, out m_FileName);
            }
            else
            {
                m_FileName = @"__Unknown\" + dwHash.ToString("X16");
            }

            return m_FileName;
        }
    }
}
