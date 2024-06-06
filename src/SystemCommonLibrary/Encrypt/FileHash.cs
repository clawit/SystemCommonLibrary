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
                var md5s = MD5.Create();
                byte[] b = md5s.ComputeHash(fs);
                fs.Close();

                string md5 = BitConverter.ToString(b).Replace("-", string.Empty);

                return md5;
            }
		}

        public static string ComputeMd5Hash(FileStream fs)
        {
            var md5s = MD5.Create();
            byte[] b = md5s.ComputeHash(fs);
            string md5 = BitConverter.ToString(b).Replace("-", string.Empty);

            return md5;
        }
	}
}
