using NUnit.Framework;
using UnityEngine;
using Assets.Logic.Util;
using System.Security.Cryptography;

namespace Assets.Tests
{
    class UtilTests
    {
        [Test]
        public void GetHashString()
        {
            Debug.Log(Util.GetHashString("Alexis123"));
        }

        [Test]
        public void Encrypt()
        {
            string original = "Here is some data to encrypt!";
            using (Aes myAes = Aes.Create())
            {

                // Encrypt the string to an array of bytes.
                byte[] encrypted = Util.EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);               
                
                string roundtrip = Util.DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

                Debug.Log("Original: "+original);
                Debug.Log("Round Trip: "+roundtrip);
            }
        }
    }
}
