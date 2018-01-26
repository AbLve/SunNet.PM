using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Core.Repository
{
    public interface IRepository<T>
    {
        int Insert(T entity);
        bool Update(T entity);
        bool Delete(int entityId); 

        T Get(int entityId);
    }
}
