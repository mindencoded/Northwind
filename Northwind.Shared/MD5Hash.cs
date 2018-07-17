using System.Security.Cryptography;
using System.Text;

namespace Northwind.Shared
{
    public class Md5Hash
    {
        public static string Create(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to hexadecimal string
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sBuilder.Append(hashBytes[i].ToString("X2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}