using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;
using SF.Framework.Mvc.Search;
using System.Data.Entity.Validation;
using SF.Framework.Log.Providers;

namespace SF.Framework.Mvc.Data
{
    public class EntityRepository<TDbContext, TEntity, TPkType> : IEntityRepository<TEntity, TPkType>
        where TEntity : class
        where TDbContext : DbContext, new()
    {
        #region Implement IEntityRepository<TEntity, TPkID>

        public DbContext CurrentDbContext
        {
            get
            {
                _DbContext = Reset();
                return _DbContext;
            }
        }
        public DbContext _DbContext = null;
        public EntityRepository()
        {
            _DbContext = new TDbContext();
        }
        protected DbSet<TEntity> DbSet
        {
            get { return this.CurrentDbContext.Set<TEntity>(); }
        }
        protected virtual DbContext Reset()
        {
            return _DbContext;
        }
        #region IEntityRepository<TEntity>

        public TEntity Create()
        {
            return this.CurrentDbContext.Set<TEntity>().Create();
        }

        public TEntity FindByID(TPkType ID)
        {
            var entity = this.CurrentDbContext.Set<TEntity>().Find(ID);
            return entity;
        }

        #region Insert

        public int Insert(TEntity entity)
        {
            this.CurrentDbContext.Set<TEntity>().Add(entity);
            // 5.0
            // this._db.Entry(entity).State = System.Data.EntityState.Added;
            this.CurrentDbContext.Entry(entity).State = System.Data.Entity.EntityState.Added;
            int i = this.SaveChanges();
            _DbContext = new TDbContext();
            return i;
        }

        public int Insert(IList<TEntity> list)
        {
            int flag = 0;
            int completedFlag = list.Count;
            using (var trans = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
            {
                list.ToList().ForEach(entity =>
                {
                    flag += this.Insert(entity);
                });
                // if object has foreignkey object,flag man be larger than 1 
                if (flag >= completedFlag)
                {
                    trans.Complete();
                    return completedFlag;
                }
                else
                    return 0;
            }
        }

        #endregion

        #region Update

        public int Update(TEntity entity)
        {
            var entry = this.CurrentDbContext.Entry(entity);
            this.CurrentDbContext.Set<TEntity>().Attach(entity);
            entry.State = System.Data.Entity.EntityState.Modified;
            return this.SaveChanges();
        }

        public int Update(TEntity entity, string pkName, params string[] columnNames)
        {
            var entry = this.CurrentDbContext.Entry(entity);
            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                //get primary key value.
                Type type = typeof(TEntity);
                FieldInfo info = type.GetField(pkName);
                object o = info.GetValue(entity);

                // entry
                var entryToUpdate = this.CurrentDbContext.Entry(this.CurrentDbContext.Set<TEntity>().Find(o));

                columnNames.ToList().ForEach(column =>
                {
                    entryToUpdate.Property(column).CurrentValue = entry.Property(column).CurrentValue;
                    entryToUpdate.Property(column).IsModified = true;
                });
            }
            return this.SaveChanges();
        }


        public int Update(IList<TEntity> list)
        {
            int flag = 0;
            int completedFlag = list.Count;
            using (var trans = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
            {
                list.ToList().ForEach(entity =>
                {
                    flag += this.Update(entity);
                });
                if (flag == completedFlag)
                {
                    trans.Complete();
                    return completedFlag;
                }
                else
                    return 0;
            }




        }

        public int Update(IList<TEntity> list, int count)
        {
            int flag = 0;
            using (var trans = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
            {
                list.ToList().ForEach(entity =>
                {
                    flag += this.Update(entity);
                });
                if (flag == list.Count - count)
                {
                    trans.Complete();
                    return flag;
                }
                else
                    return 0;
            }
        }

        public int Update(IList<TEntity> list, string pkName, params string[] columnNames)
        {
            int flag = 0;
            int completedFlag = list.Count;
            using (var trans = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
            {
                list.ToList().ForEach(entity =>
                {
                    flag += this.Update(entity, pkName, columnNames);
                });
                if (flag == completedFlag)
                {
                    trans.Complete();
                    return completedFlag;
                }
                else
                    return 0;
            }
        }
        public int Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity>> updater)
        {
            // where
            var objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)CurrentDbContext).ObjectContext;
            var set = objectContext.CreateObjectSet<TEntity>();
            var query = set.Where(where);

            //System.Data.Objects.ObjectQuery<TEntity> objQuery = query as System.Data.Objects.ObjectQuery<TEntity>;
            System.Data.Entity.Core.Objects.ObjectQuery<TEntity> objQuery = query as System.Data.Entity.Core.Objects.ObjectQuery<TEntity>;
            List<object> objParams = new List<object>();
            string sql = objQuery.ToTraceString();
            sql = sql.Substring(sql.IndexOf("from", StringComparison.OrdinalIgnoreCase)).Replace("__linq__", "");
            int paramindex = objQuery.Parameters.Count;
            foreach (var para in objQuery.Parameters)
            {
                objParams.Add(para.Value);
            }
            // update set
            var valueObj = updater.Compile().Invoke();
            MemberInitExpression updateMemberExpr = (MemberInitExpression)updater.Body;
            StringBuilder updateBuilder = new StringBuilder();
            Type valueType = typeof(TEntity);
            foreach (var bind in updateMemberExpr.Bindings.Cast<MemberAssignment>())
            {
                string name = bind.Member.Name;
                updateBuilder.AppendFormat("{0}=@p{1},", name, paramindex++);
                var value = valueType.GetProperty(name).GetValue(valueObj, null);
                objParams.Add(value);
            }
            if (updateBuilder.Length == 0)
            {
                throw new Exception("Please update at list one property.");
            }
            else
            {
                sql = " update [Extent1] set " + updateBuilder.Remove(updateBuilder.Length - 1, 1).ToString() + " " + sql;
            }
            int index = this.CurrentDbContext.Database.ExecuteSqlCommand(sql, objParams.ToArray());
            _DbContext = new TDbContext();
            return index;
        }
        #endregion

        #region Delete

        public int Delete(TEntity entity)
        {
            this.CurrentDbContext.Set<TEntity>().Remove(entity);
            this.CurrentDbContext.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            return this.SaveChanges();
        }

        public int Delete(TPkType ID)
        {
            var entity = this.CurrentDbContext.Set<TEntity>().Find(ID);
            this.CurrentDbContext.Set<TEntity>().Remove(entity);
            this.CurrentDbContext.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            return this.SaveChanges();
        }

        public int Delete(IList<TEntity> list)
        {
            int flag = 0;
            int completedFlag = list.Count;
            using (var trans = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
            {
                list.ToList().ForEach(entity =>
                {
                    flag += this.Delete(entity);
                });
                if (flag == completedFlag)
                {
                    trans.Complete();
                    return completedFlag;
                }
                else
                    return 0;
            }
        }

        public int Delete(string deleteIds)
        {
            if (deleteIds.Trim().Equals(""))
                return 0;
            ArrayList arrayId = new ArrayList();
            foreach (string id in deleteIds.Split(','))
                if (!id.Trim().Equals(""))
                    arrayId.Add(Convert.ToInt32(id.Trim()));
            return this.Delete((TPkType[])arrayId.ToArray(typeof(TPkType)));
        }

        public int Delete(params TPkType[] keyValues)
        {
            int flag = 0;
            int completedFlag = keyValues.Length;
            using (var trans = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
            {
                keyValues.ToList().ForEach(ID =>
                {
                    var entity = this.CurrentDbContext.Set<TEntity>().Find(ID);
                    flag += this.Delete(entity);
                });
                if (flag == completedFlag)
                {
                    trans.Complete();
                    return completedFlag;
                }
                else
                    return 0;
            }

        }

        //public int Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> Where)
        //{

        //    IList<TEntity> entityList = this._db.Set<TEntity>().Where(Where).ToList();
        //    return this.Delete(entityList);
        //}

        public int Delete(Expression<Func<TEntity, bool>> where)
        {
            var objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)CurrentDbContext).ObjectContext;
            var set = objectContext.CreateObjectSet<TEntity>();
            var query = set.Where(where);

            //System.Data.Objects.ObjectQuery<TEntity> objQuery = query as System.Data.Objects.ObjectQuery<TEntity>;
            System.Data.Entity.Core.Objects.ObjectQuery<TEntity> objQuery = query as System.Data.Entity.Core.Objects.ObjectQuery<TEntity>;
            string sql = objQuery.ToTraceString();

            sql = "delete " + sql.Substring(sql.IndexOf("from", StringComparison.OrdinalIgnoreCase));
            sql = sql.Replace("[Extent1].", "").Replace("AS [Extent1]", "").Replace("__linq__", "");
            List<object> objs = new List<object>();
            foreach (var para in objQuery.Parameters)
            {
                objs.Add(para.Value);
            }
            int index = this.CurrentDbContext.Database.ExecuteSqlCommand(sql, objs.ToArray());
            _DbContext = new TDbContext();
            return index;
        }

        #endregion

        #region Execute sql

        public Database GetDatabase()
        {
            return CurrentDbContext.Database;
        }

        #endregion

        #region Get list

        public DbSet<TEntity> GetList()
        {
            return this.DbSet;
        }



        #endregion

        public TEntity GetEntity(System.Linq.Expressions.Expression<Func<TEntity, bool>> Where)
        {
            return this.DbSet.Where(Where).FirstOrDefault();
        }
        public TEntity GetEntity(Core.Specifications.ISpecification<TEntity> spec)
        {
            return GetEntity(spec.GetExpression());
        }

        public bool ExistEntity(System.Linq.Expressions.Expression<Func<TEntity, bool>> Where)
        {
            return (this.GetEntity(Where) == null ? false : true);
        }

        public Mvc.Pager.PagedList<TEntity> GetList(string Where, string Order, int PageIndex = 1, int PageSize = 24, string FieldsName = "*")
        {
            string tEntityTypeString = typeof(TEntity).Name;
            string Primarykey = "ID";

            #region page parameter
            SqlParameter[] parameterObject = new SqlParameter[] {
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "TableName",
                    Value = tEntityTypeString
                },
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "Primarykey",
                    Value = Primarykey
                },
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "FieldsName",
                    Value = FieldsName
                },
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "ByWHERE",
                    Value = Where
                },
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "ByOrder",
                    Value = Order
                },
                 new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "PageSize",
                    Value = PageSize
                },
                 new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "PageIndex",
                    Value = PageIndex
                },
                new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "RecordCount",
                    Direction = ParameterDirection.Output
                },
                new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "PageCount",
                    Direction = ParameterDirection.Output
                }
            };
            #endregion

            var result = (from p in CurrentDbContext.Set<TEntity>().SqlQuery("EXECUTE Pager @TableName,@Primarykey,@FieldsName,@ByWHERE,@ByOrder,@PageSize,@PageIndex,@RecordCount output,@PageCount output", parameterObject) select p).ToList();
            int RecordCount = Convert.ToInt32(parameterObject[7].Value);
            return new Mvc.Pager.PagedList<TEntity>(result, PageIndex, PageSize, RecordCount);
        }

        public IList<TEntity> GetListByTopN(int TopN, string Where, string Order, string FieldsName = "*")
        {
            string tEntityTypeString = typeof(TEntity).Name;
            string Primarykey = "ID";

            #region page parameter
            SqlParameter[] parameterObject = new SqlParameter[] {
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "TableName",
                    Value = tEntityTypeString
                },
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "Primarykey",
                    Value = Primarykey
                },
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "FieldsName",
                    Value = FieldsName
                },
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "ByWHERE",
                    Value = Where
                },
                new SqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "ByOrder",
                    Value = Order
                },
                 new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "PageSize",
                    Value = TopN
                },
                 new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "PageIndex",
                    Value = 1
                },
                new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "RecordCount",
                    Direction = ParameterDirection.Output
                },
                new SqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "PageCount",
                    Direction = ParameterDirection.Output
                }
            };
            #endregion

            var result = (from p in CurrentDbContext.Set<TEntity>().SqlQuery("EXECUTE Pager @TableName,@Primarykey,@FieldsName,@ByWHERE,@ByOrder,@PageSize,@PageIndex,@RecordCount output,@PageCount output", parameterObject) select p).ToList();
            var list = result.ToList();
            return list;
        }


        #endregion


        #region Lambda expression data extended operation

        public Mvc.Pager.PagedList<TEntity> GetList(Expression<Func<TEntity, bool>> conditionExpression,
                                                                    string orderByString,
                                                                    string order,
                                                                    int PageIndex = 1,
                                                                    int PageSize = 24)
        {

            var query = order.Trim().ToLower().Equals("desc")
                ?
                (this.DbSet.Where(conditionExpression).OrderBy(orderByString, true))
                :
                (this.DbSet.Where(conditionExpression).OrderBy(orderByString));
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            var list = new Mvc.Pager.PagedList<TEntity>(result, PageIndex, PageSize, query.Count());
            return list;
        }

        public Mvc.Pager.PagedList<TEntity> GetList(SF.Framework.Mvc.Search.Model.QueryModel model,
                                                            string orderByString,
                                                            string order,
                                                            int PageIndex = 1,
                                                            int PageSize = 24)
        {
            var query = order.Trim().ToLower().Equals("desc")
                ?
                (this.DbSet.Where(model).OrderBy(orderByString, true))
                :
                (this.DbSet.Where(model).OrderBy(orderByString));
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            var list = new Mvc.Pager.PagedList<TEntity>(result, PageIndex, PageSize, query.Count());
            return list;
        }

        public Mvc.Pager.PagedList<TEntity> GetList<TOrder>(Expression<Func<TEntity, bool>> conditionExpression,
                                                                    Expression<Func<TEntity, TOrder>> orderByExpression,
                                                                    bool IsDESC,
                                                                    int PageIndex = 1,
                                                                    int PageSize = 24)
        {
            var query = IsDESC
                ?
                (this.DbSet.Where(conditionExpression).OrderByDescending(orderByExpression))
                :
                (this.DbSet.Where(conditionExpression).OrderBy(orderByExpression));
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            var list = new Mvc.Pager.PagedList<TEntity>(result, PageIndex, PageSize, query.Count());
            return list;
        }

        public Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   Expression<Func<TEntity, bool>> conditionExpression,
                                                                   string orderByString,
                                                                   string order,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24)
        {
            var query = order.Trim().ToLower().Equals("desc")
                ?
                (this.DbSet.Where(conditionExpression).OrderBy(orderByString, true))
                :
                (this.DbSet.Where(conditionExpression).OrderBy(orderByString));
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var list = new Mvc.Pager.PagedList<object>(selector(result), PageIndex, PageSize, query.Count());
            return list;
            #region reflection
            //PropertyInfo property = typeof(TEntity).GetProperty(orderByString);
            //string type = property.PropertyType.FullName;

            //Type generic = typeof(CreateExpression<,>);
            //Type[] typeArgs2 = { typeof(TEntity), Type.GetType(type) };
            //generic = generic.MakeGenericType(typeArgs2);
            //var dic = Activator.CreateInstance(generic);

            //MethodInfo mi = dic.GetType().GetMethod("GetExpressionByStringColumn");
            //Expression<Func<TEntity, int>> result = mi.Invoke(null, new object[] { orderByString }) as Expression<Func<TEntity, int>>;
            #endregion
        }

        public Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   Core.Specifications.ISpecification<TEntity> spec,
                                                                   string orderByString,
                                                                   string order,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24)
        {
            var query = order.Trim().ToLower().Equals("desc")
                ?
                (this.DbSet.Where(spec.GetExpression()).OrderBy(orderByString, true))
                :
                (this.DbSet.Where(spec.GetExpression()).OrderBy(orderByString));
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var list = new Mvc.Pager.PagedList<object>(selector(result), PageIndex, PageSize, query.Count());
            return list;
            #region reflection
            //PropertyInfo property = typeof(TEntity).GetProperty(orderByString);
            //string type = property.PropertyType.FullName;

            //Type generic = typeof(CreateExpression<,>);
            //Type[] typeArgs2 = { typeof(TEntity), Type.GetType(type) };
            //generic = generic.MakeGenericType(typeArgs2);
            //var dic = Activator.CreateInstance(generic);

            //MethodInfo mi = dic.GetType().GetMethod("GetExpressionByStringColumn");
            //Expression<Func<TEntity, int>> result = mi.Invoke(null, new object[] { orderByString }) as Expression<Func<TEntity, int>>;
            #endregion
        }

        public Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   SF.Framework.Mvc.Search.Model.QueryModel model,
                                                                   string orderByString,
                                                                   string order,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24)
        {
            var query = order.Trim().ToLower().Equals("desc")
                ?
                (this.DbSet.AsQueryable().Where(model).OrderBy(orderByString, true))
                :
                (this.DbSet.AsQueryable().Where(model).OrderBy(orderByString));
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var list = new Mvc.Pager.PagedList<object>(selector(result), PageIndex, PageSize, query.Count());
            return list;
            #region reflection
            //PropertyInfo property = typeof(TEntity).GetProperty(orderByString);
            //string type = property.PropertyType.FullName;

            //Type generic = typeof(CreateExpression<,>);
            //Type[] typeArgs2 = { typeof(TEntity), Type.GetType(type) };
            //generic = generic.MakeGenericType(typeArgs2);
            //var dic = Activator.CreateInstance(generic);

            //MethodInfo mi = dic.GetType().GetMethod("GetExpressionByStringColumn");
            //Expression<Func<TEntity, int>> result = mi.Invoke(null, new object[] { orderByString }) as Expression<Func<TEntity, int>>;
            #endregion
        }

        public Mvc.Pager.PagedList<object> GetList<TOrder>(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   Expression<Func<TEntity, bool>> conditionExpression,
                                                                   Expression<Func<TEntity, TOrder>> orderByExpression,
                                                                   bool IsDESC,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24)
        {
            var query = IsDESC
                ?
                (this.DbSet.Where(conditionExpression).OrderByDescending(orderByExpression))
                :
                (this.DbSet.Where(conditionExpression).OrderBy(orderByExpression));
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var list = new Mvc.Pager.PagedList<object>(selector(result), PageIndex, PageSize, query.Count());
            return list;
        }

        public Mvc.Pager.PagedList<object> GetPagedList(IQueryable<TEntity> query,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24)
        {

            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            var list = new Mvc.Pager.PagedList<object>(result, PageIndex, PageSize, query.Count());
            return list;
        }


        public IList<object> GetList<TOrder>(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   Expression<Func<TEntity, bool>> conditionExpression,
                                                                   Expression<Func<TEntity, TOrder>> orderByExpression,
                                                                   bool IsDESC)
        {
            var query = IsDESC
                ?
                (this.DbSet.Where(conditionExpression).OrderByDescending(orderByExpression))
                :
                (this.DbSet.Where(conditionExpression).OrderBy(orderByExpression));
            var list = selector(query).ToList();
            return list;
        }

        public IList<TEntity> GetListByTopN<TOrder>(int TopN, Expression<Func<TEntity, bool>> conditionExpression, Expression<Func<TEntity, TOrder>> orderByExpression, bool IsDESC)
        {
            var query = IsDESC
                ?
                this.DbSet.Where(conditionExpression).OrderByDescending(orderByExpression).Take(TopN)
                :
                this.DbSet.Where(conditionExpression).OrderBy(orderByExpression).Take(TopN);

            var list = query.ToList();
            return list;
        }

        public IList<object> GetListByTopN<TOrder>(int TopN, Func<IQueryable<TEntity>, List<object>> selector, Expression<Func<TEntity, bool>> conditionExpression, Expression<Func<TEntity, TOrder>> orderByExpression, bool IsDESC)
        {
            var query = IsDESC
                ?
                this.DbSet.Where(conditionExpression).OrderByDescending(orderByExpression).Take(TopN)
                :
                this.DbSet.Where(conditionExpression).OrderBy(orderByExpression).Take(TopN);

            var list = selector(query);
            return list;

        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> conditionExpression)
        {
            var list = this.DbSet.Where(conditionExpression).ToList();
            return list;
        }

        public IList<TEntity> GetList(System.Linq.Expressions.Expression<Func<TEntity, bool>> conditionExpression, int TopN)
        {
            var list = this.DbSet.Where(conditionExpression).Take(TopN).ToList();
            return list;
        }

        public IList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector, System.Linq.Expressions.Expression<Func<TEntity, bool>> conditionExpression)
        {
            var query = this.DbSet.Where(conditionExpression);

            var list = selector(query);
            return list;
        }

        public IList<TEntity> GetList<TOrder>(Expression<Func<TEntity, bool>> conditionExpression, Expression<Func<TEntity, TOrder>> orderByExpression, bool IsDESC)
        {
            var query = IsDESC
                ?
                (this.DbSet.Where(conditionExpression).OrderByDescending(orderByExpression))
                :
                (this.DbSet.Where(conditionExpression).OrderBy(orderByExpression));
            var list = query.ToList();
            return list;
        }

        public IList<TEntity> GetList(SF.Framework.Mvc.Search.Model.QueryModel model)
        {
            var query = (this.DbSet.AsQueryable().Where(model));
            var list = query.ToList();
            return list;
        }

        #endregion

        public int SaveChanges()
        {
            try
            {
                int i = this.CurrentDbContext.SaveChanges();
                return i;
            }
            catch (Exception dbEx)
            {
                TextFileLogger log = new TextFileLogger();
                log.Log(dbEx);
                return 0;

            }
        }


        public Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                  SF.Framework.Mvc.Search.Model.QueryModel model,
                                                                  Dictionary<string, bool> paramNames,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24)
        {
            var query = this.DbSet.Where(model).OrderBy(paramNames);
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var list = new Mvc.Pager.PagedList<object>(selector(result), PageIndex, PageSize, query.Count());
            return list;
        }
        #endregion

        #region IEntityRepository<TEntity,TPkType>


        public Pager.PagedList<object> GetList(Expression<Func<TEntity, bool>> conditionExpression
                                                , Dictionary<string, bool> paramNames
                                                , int PageIndex = 1
                                                , int PageSize = 24)
        {
            var query = this.DbSet.Where(conditionExpression).OrderBy(paramNames);
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var list = new Mvc.Pager.PagedList<object>(result, PageIndex, PageSize, query.Count());
            return list;
        }
        public Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                               Expression<Func<TEntity, bool>> conditionExpression
                                               , Dictionary<string, bool> paramNames
                                               , int PageIndex = 1
                                               , int PageSize = 24)
        {
            var query = this.DbSet.Where(conditionExpression).OrderBy(paramNames);
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var list = new Mvc.Pager.PagedList<object>(selector(result), PageIndex, PageSize, query.Count());
            return list;
        }
        public Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                              Core.Specifications.ISpecification<TEntity> spec
                                              , Dictionary<string, bool> paramNames
                                              , int PageIndex = 1
                                              , int PageSize = 24)
        {
            var query = this.DbSet.Where(spec.GetExpression()).OrderBy(paramNames);
            var result = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var list = new Mvc.Pager.PagedList<object>(selector(result), PageIndex, PageSize, query.Count());
            return list;
        }
        public void GetList(Dictionary<string, bool> paramNames)
        {
            this.DbSet.OrderBy(paramNames).Count();
        }
        #endregion



    }
}

