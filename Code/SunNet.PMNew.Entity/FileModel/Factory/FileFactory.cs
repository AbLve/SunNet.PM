using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.FileModel
{
    public class FileFactory
    {
        public static FilesEntity CreateFileEntity(int createUserID, ISystemDateTime datetimeProvider)
        {
            FilesEntity model = new FilesEntity();

            model.ID = 0;
            model.CreatedBy = createUserID;
            model.CreatedOn = datetimeProvider.Now;
            model.ModifiedBy = createUserID;
            model.ModifiedOn = datetimeProvider.Now;

            model.ContentType = string.Empty;
            model.FilePath = string.Empty;
            model.FileID = 0;
            model.FileSize = 0;
            model.FileTitle = string.Empty;
            model.IsDelete = false;
            model.IsPublic = false;
            model.FeedbackId = 0;
            model.TicketId = 0;
            model.ProjectId = 0;
            model.SourceType = 0;
            model.ThumbPath = string.Empty;

            return model;
        }

        public static DirectoryEntity CreateDirectoryEntity(int createUserID, ISystemDateTime datetimeProvider)
        {
            DirectoryEntity model = new DirectoryEntity();

            model.ID = 0;
            model.CreatedBy = createUserID;
            model.CreatedOn = datetimeProvider.Now;
            model.ModifiedBy = createUserID;
            model.ModifiedOn = datetimeProvider.Now;

            model.Title = string.Empty;
            model.Description = string.Empty;
            model.Logo = string.Empty;
            model.ParentID = 0;

            return model;
        }
    }
}
