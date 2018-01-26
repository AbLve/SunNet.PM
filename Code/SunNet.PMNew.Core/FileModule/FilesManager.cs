using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.FileModel;
using System.IO;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Core.FileModule;
using SunNet.PMNew.Framework.Core;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Core.FileModule
{
    public class FilesManager : BaseMgr
    {
        private IFilesRepository fileRepo;
        private IEmailSender emailSender;
        private ICache<FilesManager> cache;
        private IDirectoryRepository direRepo;
        private IDirectoryObjectRepository direObjRepo;

        public FilesManager(ICache<FilesManager> cache, IEmailSender emailSender,
            IFilesRepository fileRepo, IDirectoryRepository direRepo,
            IDirectoryObjectRepository direObjRepo)
        {
            this.emailSender = emailSender;
            this.cache = cache;
            this.fileRepo = fileRepo;
            this.direRepo = direRepo;
            this.direObjRepo = direObjRepo;
        }
        public int AddFile(FilesEntity entity)
        {
            this.ClearBrokenRuleMessages();

            BaseValidator<FilesEntity> validator = new FilesValidator();

            if (!validator.Validate(entity))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }

            int id = fileRepo.Insert(entity);

            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }

            return id;
        }


        public bool Delete(int Sid, FileSourceType type)
        {
            this.ClearBrokenRuleMessages();
            bool result = fileRepo.Delete(Sid, type);
            if (!result)
            {
                this.AddBrokenRuleMessage();
            }
            return result;
        }
        public bool DeleteFile(int id)
        {
            this.ClearBrokenRuleMessages();
            bool result = fileRepo.Delete(id);
            if (!result)
            {
                this.AddBrokenRuleMessage();
            }
            return result;
        }

        #region IFilesRepository Members

        public enum WaterMark
        {
            None,
            Text,
            Image,
            Both
        }

        #endregion
        /// <summary>
        /// GetFiles without Sunnet Company
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<FileDetailDto> GetFiles(SearchFilesRequest request)
        {
            this.ClearBrokenRuleMessages();
            List<FileDetailDto> list = fileRepo.GetFileList(request);
            if (list == null)
            {
                this.AddBrokenRuleMessage();
            }
            return list;
        }

        public FilesEntity Get(int fileId)
        {
            return fileRepo.Get(fileId);
        }
        private const string CACHE_ALLDIRECTORIES = "Directories";
    }
}
