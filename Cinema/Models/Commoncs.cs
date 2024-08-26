using System.Security.Cryptography;
using System.Text;

namespace Cinema.Models
{
    public class Commoncs
    {
        //Mã hóa thành chuỗiMD5
        //Inout: sToEncrpt( chuỗi cần mã hóa)
        //Output: CHuỗi sau khi mã hóa
        public static string Hash(string text)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder hashSb = new StringBuilder();
            foreach (byte b in hash)
            {
                hashSb.Append(b.ToString("X2"));
            }
            return hashSb.ToString();
        }
    }
}
