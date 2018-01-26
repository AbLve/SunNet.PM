using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SF.Framework.Utils.Providers
{
    class MD5Encrypt : IEncrypt
    {
        public string Encrypt(string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            byte[] output = md5.ComputeHash(bytes);

            return BitConverter.ToString(output);
        }

        public string Decrypt(string source)
        {
            throw new NotImplementedException();
        }
    }
}
