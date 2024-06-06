﻿using System;
using System.Text;
using System.Security.Cryptography;

namespace SystemCommonLibrary.Encrypt
{
	public static class Md5Encrypt
	{
		public static string Md5 (string content)
		{
			byte [] input = Encoding.UTF8.GetBytes (content);
			
			var md5 = MD5.Create();
			byte [] output = md5.ComputeHash (input);

			return BitConverter.ToString (output);
		}
	}
}
