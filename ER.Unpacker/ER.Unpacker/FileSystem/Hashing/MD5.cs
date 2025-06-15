using System;
using System.Text;
using System.Security.Cryptography;

namespace ER.Unpacker
{
    class MD5
    {
        public static UInt64 iGetHash(String m_String)
        {
            MD5CryptoServiceProvider TMD5 = new MD5CryptoServiceProvider();
            var lpHash = TMD5.ComputeHash(new ASCIIEncoding().GetBytes(m_String));

            return BitConverter.ToUInt64(lpHash, 0);
        }
    }
}
