using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.FileModule;
using StructureMap;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Framework.Utils;
using System.IO;

namespace SunNet.PMNew.App
{
    public class FileApplication : BaseApp
    {
        private FilesManager mgr;
        private IFilesRepository repository;

        public FileApplication()
        {
            mgr = new FilesManager(ObjectFactory.GetInstance<ICache<FilesManager>>(),
                                    ObjectFactory.GetInstance<IEmailSender>(),
                                    ObjectFactory.GetInstance<IFilesRepository>(),
                                    ObjectFactory.GetInstance<IDirectoryRepository>(),
                                    ObjectFactory.GetInstance<IDirectoryObjectRepository>()
                                    );
            repository = ObjectFactory.GetInstance<IFilesRepository>();
        }
        public int AddFile(FilesEntity file)
        {
            this.ClearBrokenRuleMessages();
            int result = mgr.AddFile(file);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }
        public List<FilesEntity> GetFileListBySourceId(int Sid, FileSourceType SourceType)
        {
            this.ClearBrokenRuleMessages();

            if (Sid <= 0) return null;

            return repository.GetFileListBySourceId(Sid, SourceType);
        }
        public bool DeleteFile(int Sid, FileSourceType type)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.Delete(Sid, type);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public bool DeleteFile(int id)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.DeleteFile(id);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }
        public List<FileDetailDto> GetFiles(SearchFilesRequest request)
        {
            this.ClearBrokenRuleMessages();
            List<FileDetailDto> list = mgr.GetFiles(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public FilesEntity Get(int fileId)
        {
            return mgr.Get(fileId);
        }

    }
}
