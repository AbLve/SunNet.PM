using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyBook.Entity;

namespace FamilyBook.Core.CommentModule
{
    public class CommentService
    {
        private ICommentDAO m_dao;

        public CommentService(ICommentDAO dao)
        {
            m_dao = dao;
        }

        public int Insert(CommentEntity entity)
        {
            return m_dao.Insert(entity);
        }

        public bool Update(CommentEntity entity)
        {
            return m_dao.Update(entity);
        }

        public bool Delete(int id)
        {
            return m_dao.Delete(id);
        }

        public CommentEntity Get(int id)
        {
            return m_dao.Get(id);
        }

        public List<CommentEntity> GetList(Func<CommentEntity, bool> f)
        {
            return m_dao.GetList(f);
        }

        public List<CommentEntity> GetListByDynamic(string sql)
        {
            return m_dao.GetListByDynamic(sql);
        }
    }
}
