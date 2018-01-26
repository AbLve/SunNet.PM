using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Framework.Utils
{
    public interface IStringZipper
    {
        string Zip(string uncompressedString);
        string UnZip(string compressedString);
    }
}