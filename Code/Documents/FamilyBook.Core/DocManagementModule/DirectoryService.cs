using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Core.DocManagementModule
{
    public class DirectoryService
    {
        IDirectoryDAO dao;
        public DirectoryService(IDirectoryDAO dDao)
        {
            dao = dDao;
        }
    }
}
