using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.StringZipper
{
    public interface IStringZipper
    {
        string Zip(string uncompressedString);
        string UnZip(string compressedString);
    }
}