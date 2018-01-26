using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace SF.Framework.Encrypt.Providers
{
    public class DESEncrypt : IEncrypt
    {
        public string Encrypt(string source)
        {
            string key = ".!e@0Na&";
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] bytes = Encoding.UTF8.GetBytes(source);

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();

            StringBuilder sb = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }

        public string Decrypt(string source)
        {
            if (source == null || source.Length == 0)
            {
                return source;
            }

            string key = ".!e@0Na&";
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] bytes = new byte[source.Length / 2];
                for (int x = 0; x < source.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(source.Substring(x * 2, 2), 16));
                    bytes[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
