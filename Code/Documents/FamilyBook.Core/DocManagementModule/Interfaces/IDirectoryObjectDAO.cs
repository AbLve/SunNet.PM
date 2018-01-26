using FamilyBook.Entity.DocManagements;
using SF.Framework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Core.DocManagementModule
{
    public interface IDirectoryObjectDAO : IRepository<DirectoryObjectsEntity>
    {
        List<DirectoryEntity> GetObjects(int parentID);
        int ChangeParent(string objects, int parentid);
    }
}
