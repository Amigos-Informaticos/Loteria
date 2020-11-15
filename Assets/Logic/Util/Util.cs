using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Logic.Util
{
    public class Util
    {
        public static string Encrypt(string _stringToEncrypt)
        {
            string result = string.Empty;
            byte[] encrypted = Encoding.Unicode.GetBytes(_stringToEncrypt);
            int count = 0;
            while(count < _stringToEncrypt.Length)  
            {
                encrypted = Encoding.Unicode.GetBytes(Convert.ToBase64String(encrypted));
                count++;
            }
            result = Convert.ToBase64String(encrypted);

            return result;
        }
    }
}
