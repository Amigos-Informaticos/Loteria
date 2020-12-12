using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class Util
{
	private static byte[] GetHash(string inputString)
	{
		using (HashAlgorithm algorithm = SHA256.Create())
		{
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
		}
	}

	public static string GetHashString(string inputString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in GetHash(inputString))
		{
			if (stringBuilder.ToString().Length < 33)
			{
				stringBuilder.Append(b.ToString("X2"));
			}
		}
		return stringBuilder.ToString();
	}

	public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
	{
		if (plainText == null || plainText.Length <= 0)
		{
			throw new ArgumentNullException("plainText");
		}
		if (key == null || key.Length <= 0)
		{
			throw new ArgumentNullException("key");
		}
		if (iv == null || iv.Length <= 0)
		{
			throw new ArgumentNullException("iv");
		}
		byte[] encrypted;

		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = key;
			aesAlg.IV = iv;

			ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
			using (MemoryStream msEncrypt = new MemoryStream())
			{
				using (CryptoStream csEncrypt =
					new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
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

	public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] iv)
	{
		if (cipherText == null || cipherText.Length <= 0)
		{
			throw new ArgumentNullException("cipherText");
		}
		if (key == null || key.Length <= 0)
		{
			throw new ArgumentNullException("key");
		}
		if (iv == null || iv.Length <= 0)
		{
			throw new ArgumentNullException("iv");
		}
		string plaintext = null;
		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = key;
			aesAlg.IV = iv;
			ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

			using (MemoryStream memoryStreamDecrypt = new MemoryStream(cipherText))
			{
				using (CryptoStream cryptoStreamDecrypt =
					new CryptoStream(memoryStreamDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (StreamReader streamReaderDecrypt = new StreamReader(cryptoStreamDecrypt))
					{
						plaintext = streamReaderDecrypt.ReadToEnd();
					}
				}
			}
		}
		return plaintext;
	}
}