using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Encrypt
{
    public interface IEncrypt
    {
        string Encrypt(string source);
        string Decrypt(string source);
    }
}
