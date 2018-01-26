using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.File
{
    public class FileEntity
    {
        public string DisplayName { get; set; }
        public string DbName { get; set; }
        public int Size { get; set; }
        public DateTime CreateTime { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public string ImgPath { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
       

    }
}
