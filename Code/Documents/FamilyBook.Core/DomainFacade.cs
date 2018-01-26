using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyBook.Core.CommentModule;
using StructureMap;
using FamilyBook.Core.DocManagementModule;
using FamilyBook.Core.Common;

namespace FamilyBook.Core
{
    public class DomainFacade
    {
        public static DirectoryObjectService CreateDirectoryObjectService()
        {
            DirectoryObjectService mgr = new DirectoryObjectService(ObjectFactory.GetInstance<IDirectoryObjectDAO>());
            return mgr;
        }

        public static DirectoryService CreateDirectoryService()
        {
            DirectoryService mgr = new DirectoryService(ObjectFactory.GetInstance<IDirectoryDAO>());
            return mgr;
        }

        public static FileService CreateFileService()
        {
            FileService mgr = new FileService(ObjectFactory.GetInstance<IFileDAO>());
            return mgr;
        }

        public static DocManagementService CreateDocManagementService()
        {
            DocManagementService mgr = new DocManagementService(ObjectFactory.GetInstance<IDocManagementDAO>());
            return mgr;
        }

        public static CommentService CreateCommentService()
        {
            CommentService mgr = new CommentService(ObjectFactory.GetInstance<ICommentDAO>());
            return mgr;
        }

    }
}
