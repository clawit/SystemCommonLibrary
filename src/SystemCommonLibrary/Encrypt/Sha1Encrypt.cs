using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SystemCommonLibrary.Encrypt
{
	public static class Sha1Encrypt
	{
		public static string Sha1 (string content)
		{
			byte [] input = Encoding.UTF8.GetBytes (content);
			SHA1 sha1 = new SHA1CryptoServiceProvider ();
			byte [] output = sha1.ComputeHash (input);

			return BitConverter.ToString (output);
		}
	}
}
