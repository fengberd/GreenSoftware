using System.Text;

namespace 绿色软件
{
    public class MD5
    {
        private static System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        public static string HashString(string sourceString) 
        {
            return HashString(Encoding.UTF8, sourceString);
        }
        
        public static string HashString(Encoding encode, string sourceString) 
        {
            byte[] source = md5.ComputeHash(encode.GetBytes(sourceString));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
