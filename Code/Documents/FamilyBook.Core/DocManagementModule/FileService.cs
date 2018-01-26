using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Core.DocManagementModule
{
    public class FileService
    {
        IFileDAO dao;
        public FileService(IFileDAO fDao)
        {
            dao = fDao;
        }
    }
}
