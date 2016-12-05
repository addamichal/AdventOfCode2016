using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "abbhdwsy";

            Console.WriteLine("Decrypting first password");

            var decryptedPassword = "";
            int i = 0;
            using (MD5 md5Hash = MD5.Create())
            {
                while (true)
                {
                    var str = input + i;
                    var hash = GetMd5Hash(md5Hash, str);
                    if (hash.StartsWith("00000"))
                    {
                        if (decryptedPassword.Length == 8)
                            break;

                        decryptedPassword += hash[5];
                        Console.WriteLine(decryptedPassword.PadRight(8, '_'));
                    }

                    i++;
                }
            }

            Console.WriteLine("First password decrypted: {0}", decryptedPassword);

            Console.WriteLine("Decrypting second password");

            var decryptedPassword2 = "________";
            i = 0;
            using (MD5 md5Hash = MD5.Create())
            {
                while (true)
                {
                    var str = input + i;
                    var hash = GetMd5Hash(md5Hash, str);
                    if (hash.StartsWith("00000"))
                    {
                        int position;
                        var isNumber = int.TryParse(hash[5].ToString(), out position);
                        if (isNumber)
                        {
                            var character = hash[6];

                            if (position < decryptedPassword2.Length && decryptedPassword2[position] == '_')
                            {
                                var charArray = decryptedPassword2.ToCharArray();
                                charArray[position] = character;
                                decryptedPassword2 = new string(charArray);

                                Console.WriteLine(decryptedPassword2);
                            }
                        }

                        if (decryptedPassword2.Contains("_") == false)
                            break;
                    }

                    i++;
                }
            }

            Console.WriteLine("Second password decrypted: {0}", decryptedPassword2);
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
