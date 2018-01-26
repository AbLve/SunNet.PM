using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyBook.Entity.DocManagements;

#region Version Info
/* ========================================================================
*
* Author: lynnm Date: 2013/12/2 9:49:17
* File Name: DocManagementService
* Version: 4.0.30319.1008
*
* ========================================================================
*/
#endregion
namespace FamilyBook.Core.DocManagementModule
{
    public class DocManagementService
    {
        IDocManagementDAO dao;
        public DocManagementService(IDocManagementDAO docDAO)
        {
            this.dao = docDAO;
        }

        public DocManagementEntity GetEntity(int id)
        {
            return dao.Get(id);
        }

        public int Insert(DocManagementEntity entity)
        {
            return dao.Insert(entity);
        }

        public bool Update(DocManagementEntity entity)
        {
            return dao.Update(entity);
        }

        public bool Delete(int entityId)
        {
            return dao.Delete(entityId);
        }

        public bool Delete(int projectId, int parentId)
        {
            return dao.Delete(projectId, parentId);
        }

        public DocManagementEntity Get(int entityId)
        {
            return dao.Get(entityId);
        }

        public List<DocManagementEntity> GetList(int parentId)
        {
            return dao.GetList(parentId);
        }

        public List<DocManagementEntity> GetList(int parentId, string order = "", string sort = "")
        {
            return dao.GetList(parentId, order, sort);
        }

        public List<DocManagementEntity> GetList(int projectId, int parentId, string order = "", string sort = "")
        {
            return dao.GetList(projectId, parentId, order, sort);
        }

        public List<DocManagementEntity> GetList(string projectId, int parentid, int userId, string order = "", string sort = "")
        {
            return dao.GetList(projectId,parentid, userId, order, sort);
        }

        public List<DocManagementEntity> GetList(string filename)
        {
            return dao.GetList(filename);
        }

        public List<DocManagementEntity> GetList(string projectId,int userId, string filename)
        {
            return dao.GetList(projectId,userId, filename);
        }

        public List<DocManagementEntity> GetList(int projectId, string filename)
        {
            return dao.GetList(projectId, filename);
        }

        public List<DocManagementEntity> GetListByParentID(int userId, int parentId)
        {
            return dao.GetListByParentID(userId, parentId);
        }

        public List<DocManagementEntity> GetListByParentID(int userId, int parentId, int projectId, string filename)
        {
            return dao.GetListByParentID(userId, parentId,projectId, filename);
        }
        public List<DocManagementEntity> GetListByUserId(int userId)
        {
            return dao.GetListByUserId(userId);
        }
    }
}
