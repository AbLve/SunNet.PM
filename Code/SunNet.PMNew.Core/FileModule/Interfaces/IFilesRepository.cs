using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using System.IO;
using SunNet.PMNew.Entity.FileModel;

namespace SunNet.PMNew.Core.FileModule
{
    public interface IFilesRepository : IRepository<FilesEntity>
    {

        List<FilesEntity> GetFileListBySourceId(int Sid, FileSourceType sType);
        List<FileDetailDto> GetFileList(SearchFilesRequest request);
        bool Delete(int Sid, FileSourceType type);
    }
}
