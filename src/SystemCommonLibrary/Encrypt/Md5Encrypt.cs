using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SystemCommonLibrary.Encrypt
{
	public static class Md5Encrypt
	{
		public static string Md5 (string content)
		{
			byte [] input = Encoding.UTF8.GetBytes (content);
			MD5 md5 = new MD5CryptoServiceProvider ();
			byte [] output = md5.ComputeHash (input);

			return BitConverter.ToString (output);
		}
	}
}
