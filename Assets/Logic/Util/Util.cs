using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Util
{
    public static byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string GetHashString(string inputString)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in GetHash(inputString))
        {
            if (stringBuilder.ToString().Length < 33)
                stringBuilder.Append(b.ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {        
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return encrypted;
    }

    public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        string plaintext = null;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream MemoryStreamDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream CryptoStreamDecrypt = new CryptoStream(MemoryStreamDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader StreamReaderDecrypt = new StreamReader(CryptoStreamDecrypt))
                    {
                        plaintext = StreamReaderDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }
}

