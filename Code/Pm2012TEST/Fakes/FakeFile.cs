using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.FileModel;

namespace Pm2012TEST.Fakes
{
    public class FakeFile : FilesEntity
    {
        public FilesEntity Create(int Uid)
        {
            FilesEntity model = new FilesEntity();
            model.FileID = 1;
            model.FilePath = "aa";
            model.FileSize = 11;
            if (Uid == 1)
            {
                model.FileTitle = "aaaa1111.docssss";
            }
            else
            {
                model.FileTitle = "aaaa1111.doc";
            }

            model.IsPublic = true;
            model.ID = 1;
            model.IsDelete = false;
            model.ModifiedBy = Uid;
            model.ModifiedOn = DateTime.Now;
            model.SourceID = 1;
            model.SourceType = 1;
            model.ThumbPath = "111";
            return model;
        }
    }
}
