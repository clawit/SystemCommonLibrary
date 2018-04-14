using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace SystemCommonLibrary.Encrypt
{
    public static class FileHash
	{
		public static string ComputeMd5Hash (string file)
		{
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 8192))
            {
                MD5CryptoServiceProvider md5csp = new MD5CryptoServiceProvider();
                md5csp.ComputeHash(fs);
                fs.Close();

                byte[] b = md5csp.Hash;
                string md5 = BitConverter.ToString(b).Replace("-", string.Empty);

                return md5;
            }
		}

        public static string ComputeMd5Hash(FileStream fs)
        {
            MD5CryptoServiceProvider md5csp = new MD5CryptoServiceProvider();
            md5csp.ComputeHash(fs);

            byte[] b = md5csp.Hash;
            string md5 = BitConverter.ToString(b).Replace("-", string.Empty);

            return md5;
        }
	}
}
