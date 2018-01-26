using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Framework.Utils
{
    public interface IFile
    {
        void Move(string source, string destination);
        void Copy(string source, string destination);
        void Delete(string filePath);
    }
}
