using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Framework.Utils.Providers
{
    public class RealFileSystem:IFile
    {
        public void Move(string source, string destination)
        {
            System.IO.File.Move(source, destination);
        }

        public void Copy(string source, string destination)
        {
            System.IO.File.Copy(source, destination);
        }

        public void Delete(string filePath)
        {
            System.IO.File.Delete(filePath);
        }
    }
}
