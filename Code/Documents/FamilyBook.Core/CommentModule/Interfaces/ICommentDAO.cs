using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SF.Framework.Core.Repository;
using FamilyBook.Entity;

namespace FamilyBook.Core.CommentModule
{
    public interface ICommentDAO : IRepository<CommentEntity>
    {
        List<CommentEntity> GetList(Func<CommentEntity, bool> f);
        List<CommentEntity> GetListByDynamic(string sql);
    }
}
