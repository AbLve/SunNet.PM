using FamilyBook.Core.DocManagementModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Core.DocManagementModule
{
    public class DirectoryObjectService
    {
        IDirectoryObjectDAO dao;
        public DirectoryObjectService(IDirectoryObjectDAO dDao)
        {
            dao = dDao;
        }
    }
}
