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
            result = Convert.ToBase64String(encrypted);
            return result;
        }
    }
}
