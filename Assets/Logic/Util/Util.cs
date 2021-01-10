using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Util
{
	public static string PrintArrayBi(bool[,] matrix)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				stringBuilder.Append(Convert.ToInt32(matrix[i, j]) + " ");
			}
			stringBuilder.Append("\n");
		}
		return stringBuilder.ToString();
	}
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
			if (stringBuilder.ToString().Length <= 30)
				stringBuilder.Append(b.ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static Color GetHexColor(string hexadecimal)
	{
		ColorUtility.TryParseHtmlString(hexadecimal, out Color color);
		return color;
	}
}