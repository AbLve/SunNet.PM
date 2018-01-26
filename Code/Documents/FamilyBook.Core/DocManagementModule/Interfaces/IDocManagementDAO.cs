using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Core.Repository;
using FamilyBook.Entity.DocManagements;

namespace FamilyBook.Core.DocManagementModule
{
    public interface IDocManagementDAO : IRepository<DocManagementEntity>
    {
        bool Delete(int projectId, int parentId);
        List<DocManagementEntity> GetList(int parentId);
        List<DocManagementEntity> GetList(string filename);
        List<DocManagementEntity> GetList(int projectId, string filename);
        List<DocManagementEntity> GetList(string projectId, int userId, string filename);
        List<DocManagementEntity> GetList(int parentId, string order = "", string sort = "");
        List<DocManagementEntity> GetList(int projectId, int parentId, string order = "", string sort = "");
        List<DocManagementEntity> GetList(string projectId, int parentid, int userId, string order = "", string sort = "");

        List<DocManagementEntity> GetListByUserId(int userId);
        List<DocManagementEntity> GetListByParentID(int userId, int parentId);
        List<DocManagementEntity> GetListByParentID(int userId, int parentId, int projectId, string filename);
    }
}
