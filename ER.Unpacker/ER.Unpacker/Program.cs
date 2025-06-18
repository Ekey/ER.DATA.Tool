using System;
using System.IO;

namespace ER.Unpacker
{
    class Program
    {
        private static String m_Title = "Earth Revival IDX/DATA Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2025 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 1 && args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    ER.Unpacker <m_IdxFile> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of IDX file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    ER.Unpacker E:\\Games\\ER\\StreamingAssets\\datas\\data.32.idx");
                Console.WriteLine("    ER.Unpacker E:\\Games\\ER\\StreamingAssets\\datas\\data.32.idx D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_IdxFile = args[0];
            String m_Output = null;

            if (args.Length == 2)
            {
                m_Output = Utils.iCheckArgumentsPath(args[1]);
            }
            else
            {
                m_Output = Path.GetDirectoryName(args[0]) + @"\" + Path.GetFileNameWithoutExtension(args[0]) + @"\";
            }

            if (!File.Exists("UnityPlayer.dll"))
            {
                throw new Exception("[ERROR]: Unable to find UnityPlayer.dll library!");
            }
            else
            {
                if (!JNTE.iLoadUnityLibrary())
                {
                    throw new Exception("[ERROR]: Unable to load UnityPlayer.dll library!");
                }
            }

            if (!File.Exists(m_IdxFile))
            {
                throw new Exception("[ERROR]: Input Idx file -> " + m_IdxFile + " <- does not exist!");
            }

            IdxUnpack.iDoIt(m_IdxFile, m_Output);
        }
    }
}
