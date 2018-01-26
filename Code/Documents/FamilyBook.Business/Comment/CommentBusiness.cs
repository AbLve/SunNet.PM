using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyBook.Core;
using FamilyBook.Core.CommentModule;
using FamilyBook.Entity;

namespace FamilyBook.Business.Comment
{
    public class CommentBusiness
    {
        private CommentService m_CommentService = DomainFacade.CreateCommentService();

        //可以对现有的方法进行重载
        //当牵扯到CommentEntity时，方法最后必须调用Service，方法前和中，可以添加一些逻辑筛选；
        //若牵扯到其他类，最后的方法不必调用Service.

        public int Insert(CommentEntity entity)
        {
            return m_CommentService.Insert(entity);
        }

        public bool Update(CommentEntity entity)
        {
            return m_CommentService.Update(entity);
        }

        public bool Delete(int id)
        {
            return m_CommentService.Delete(id);
        }

        public CommentEntity Get(int id)
        {
            return m_CommentService.Get(id);
        }

        public List<CommentEntity> GetList(Func<CommentEntity, bool> f)
        {
            return m_CommentService.GetList(f);
        }

        public List<CommentEntity> GetListByDynamic(string sql)
        {
            return m_CommentService.GetListByDynamic(sql);
        }
    }
}
