using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using SF.Framework.Core.Specifications;

namespace SF.Framework.Mvc.Data
{
    /// <summary>    
    /// Mvc data operation development
    /// </summary>
    public interface IEntityRepository<TEntity, TPkID> where TEntity : class
    {
        /// <summary>
        /// Create Object
        /// </summary>
        /// <returns>Generic Model</returns>
        TEntity Create();

        /// <summary>
        /// According to the linq query condition judgment whether there are entities
        /// </summary>
        /// <param name="Where">linq query</param>
        /// <returns>Whether there is</returns>
        bool ExistEntity(System.Linq.Expressions.Expression<Func<TEntity, bool>> Where);

        /// <summary>        
        /// Get all entity.
        /// </summary>        
        /// <returns></returns>        
        DbSet<TEntity> GetList();

        /// <summary>
        /// FindByID
        /// </summary>
        /// <param name="ID">To find the object id.</param>
        /// <returns></returns>
        TEntity FindByID(TPkID ID);

        /// <summary>
        /// Get entity by linq.
        /// </summary>
        /// <param name="Where"></param>
        /// <returns>entity</returns>
        TEntity GetEntity(System.Linq.Expressions.Expression<Func<TEntity, bool>> Where);

        TEntity GetEntity(ISpecification<TEntity> spec);

        #region Insert

        /// <summary>       
        /// Insert entity.
        /// </summary>        
        int Insert(TEntity entity);

        /// <summary>        
        /// Insert entity list.
        /// </summary>        
        int Insert(IList<TEntity> list);

        #endregion

        #region Update

        /// <summary>        
        /// Update entity to database.
        /// </summary>        
        /// <param name="entity"></param>     
        int Update(TEntity entity);

        /// <summary>        
        /// Update entity by some fields.
        /// </summary>        
        /// <param name="entity"></param>        
        /// <param name="columnNames"></param>      
        int Update(TEntity entity, string pkName, params string[] columnNames);

        /// <summary>        
        /// Update entity list.      
        /// </summary>        
        /// <param name="entity"></param> 
        int Update(IList<TEntity> list);

        /// <summary>
        /// According to the database entity list and influenced by a number of lines record update.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count">Think successful record number</param>
        /// <returns></returns>
        int Update(IList<TEntity> list, int count);

        /// <summary>       
        /// According to the response of the attribute name on the batch update.
        /// </summary>        
        /// <param name="list"><IDataEntity></param>        
        /// <param name="columnNames"></param>     
        int Update(IList<TEntity> list, string pkName, params string[] columnNames);
        int Update(Expression<Func<TEntity, bool>> where,Expression<Func<TEntity>> updater);
        #endregion

        #region Delete

        /// <summary>       
        /// Delete one record by entity.
        /// </summary>        
        /// <param name="entity"></param>  
        int Delete(TEntity entity);

        /// <summary>       
        /// Delete one record by id.
        /// </summary>        
        /// <param name="ID"></param>  
        int Delete(TPkID ID);

        /// <summary>        
        /// Delete entity list.
        /// </summary>        
        /// <param name="list"></param>        
        int Delete(IList<TEntity> list);

        /// <summary>
        /// Delete entity list by string ids.
        /// </summary>
        /// <param name="deleteIds">You want to delete the IDs with multiple Id, use "," no separation</param>
        int Delete(string deleteIds);

        /// <summary>
        /// Delete entity list by id parameters.
        /// </summary>
        /// <param name="keyValues">id array</param>
        int Delete(params TPkID[] keyValues);

        /// <summary>
        /// Delelte entity by linq.
        /// </summary>
        /// <param name="Where"></param>
        /// <returns></returns>
        //int Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> Where);
       
        int Delete(Expression<Func<TEntity, bool>> where);
        #endregion

        #region Execute sql

        Database GetDatabase();

        #endregion

        #region Get entity list by page

        /// <summary>
        /// Paging acquisition entity list.
        /// </summary>
        /// <param name="Where"></param>
        /// <param name="Order"></param>
        /// <param name="PageIndex">Current page</param>
        /// <param name="PageSize">Count record</param>
        /// <param name="FieldsName">Show fields name</param>
        /// <returns></returns>
        Mvc.Pager.PagedList<TEntity> GetList(string Where, string Order, int PageIndex, int PageSize, string FieldsName);

        /// <summary>
        /// According to the condition expression obtained the number of specified data
        /// </summary>
        /// <param name="TopN"></param>
        /// <param name="Where"></param>
        /// <param name="Order"></param>
        /// <param name="FieldsName"></param>
        /// <returns></returns>
        IList<TEntity> GetListByTopN(int TopN, string Where, string Order, string FieldsName);

        /// <summary>
        /// According to the condition expression obtained the number of specified data
        /// </summary>
        /// <param name="TopN"></param>
        /// <param name="condition">Lambda expression</param>
        /// <param name="orderByExpression">Lambda expression</param>
        /// <param name="IsDESC">is desc</param>
        /// <returns></returns>
        IList<TEntity> GetListByTopN<TOrder>(int TopN, Expression<Func<TEntity, bool>> conditionExpression, Expression<Func<TEntity, TOrder>> orderByExpression, bool IsDESC);

        /// <summary>
        /// According to the condition expression obtained the number of specified data
        /// </summary>
        /// <param name="TopN"></param>
        /// <param name="selector">For column said anonymous object</param>
        /// <param name="condition">Lambda expression</param>
        /// <param name="orderByExpression">Lambda expression</param>
        /// <param name="IsDESC">Is desc</param>
        /// <returns></returns>
        IList<object> GetListByTopN<TOrder>(int TopN, Func<IQueryable<TEntity>, List<object>> selector, Expression<Func<TEntity, bool>> conditionExpression, Expression<Func<TEntity, TOrder>> orderByExpression, bool IsDESC);

        /// <summary>
        /// According to the condition expression obtain the relevant data
        /// </summary>
        /// <param name="condition">Lambda expression</param>
        /// <returns></returns>
        IList<TEntity> GetList(System.Linq.Expressions.Expression<Func<TEntity, bool>> conditionExpression);


        /// <summary>
        /// According to the condition expression obtain the relevant data
        /// </summary>
        /// <param name="condition">Lambda expression</param>
        /// <returns></returns>
        IList<TEntity> GetList(System.Linq.Expressions.Expression<Func<TEntity, bool>> conditionExpression, int TopN);

        /// <summary>
        /// According to the condition expression obtain the relevant data
        /// </summary>
        /// <param name="condition">Lambda expression</param>
        /// <returns></returns>
        IList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector, System.Linq.Expressions.Expression<Func<TEntity, bool>> conditionExpression);

        /// <summary>
        /// According to the condition expression obtain the relevant data
        /// </summary>
        /// <param name="condition">Lambda expression</param>
        /// <param name="orderByExpression"></param>
        /// <param name="IsDESC"></param>
        /// <returns></returns>
        IList<TEntity> GetList<TOrder>(Expression<Func<TEntity, bool>> conditionExpression, Expression<Func<TEntity, TOrder>> orderByExpression, bool IsDESC);

        /// <summary>
        /// According to the need to get the data field data paging method
        /// </summary>
        /// <param name="selector">For column said anonymous object</param>
        /// <param name="conditionExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="IsDESC"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        Mvc.Pager.PagedList<object> GetList<TOrder>(Func<IQueryable<TEntity>, List<object>> selector,
                                                            Expression<Func<TEntity, bool>> conditionExpression,
                                                            Expression<Func<TEntity, TOrder>> orderByExpression,
                                                            bool IsDESC,
                                                            int PageIndex = 1,
                                                            int PageSize = 24);

        /// <summary>
        /// According to the dynamic dynamic Query get paging object
        /// </summary>
        /// <param name="query">dynamic Query</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        Mvc.Pager.PagedList<object> GetPagedList(IQueryable<TEntity> query,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24);


        /// <summary>
        /// According to the need to get the data field data paging method
        /// </summary>
        /// <param name="selector">For column said anonymous object</param>
        /// <param name="conditionExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="IsDESC"></param>
        /// <returns></returns>
        IList<object> GetList<TOrder>(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   Expression<Func<TEntity, bool>> conditionExpression,
                                                                   Expression<Func<TEntity, TOrder>> orderByExpression,
                                                                   bool IsDESC);


        /// <summary>
        /// According to the need to get the data field data paging method
        /// </summary>
        /// <param name="selector">For column said anonymous object</param>
        /// <param name="conditionExpression"></param>
        /// <param name="orderByField">field name</param>
        /// <param name="orderSort">desc or asc</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   Expression<Func<TEntity, bool>> conditionExpression,
                                                                   string orderByField,
                                                                   string orderSort,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24);


        /// <summary>
        /// According to the need to get the data field data paging method
        /// </summary>
        /// <param name="selector">For column said anonymous object</param>
        /// <param name="spec"> Core.Specifications.ISpecification TEntity </param>
        /// <param name="orderByField">field name</param>
        /// <param name="orderSort">desc or asc</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   Core.Specifications.ISpecification<TEntity> spec,
                                                                   string orderByString,
                                                                   string order,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24);
        /// <summary>
        /// Intelligent query data paging method
        /// </summary>
        /// <param name="selector">For column said anonymous object</param>
        /// <param name="model">Search model</param>
        /// <param name="orderByField">field name</param>
        /// <param name="orderSort">desc or asc</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   SF.Framework.Mvc.Search.Model.QueryModel model,
                                                                   string orderByField,
                                                                   string orderSort,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24);

        /// <summary>
        /// Get all the field data paging method
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="IsDESC"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        Mvc.Pager.PagedList<TEntity> GetList<TOrder>(Expression<Func<TEntity, bool>> conditionExpression,
                                                                    Expression<Func<TEntity, TOrder>> orderByExpression,
                                                                    bool IsDESC,
                                                                    int PageIndex = 1,
                                                                    int PageSize = 24);

        /// <summary>
        /// Get all the field data paging method
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <param name="orderByField"></param>
        /// <param name="orderSort"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        Mvc.Pager.PagedList<TEntity> GetList(Expression<Func<TEntity, bool>> conditionExpression,
                                                                    string orderByField,
                                                                    string orderSort,
                                                                    int PageIndex = 1,
                                                                    int PageSize = 24);

        /// <summary>
        /// Get all the field data paging method
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <param name="orderByField"></param>
        /// <param name="orderSort"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        Mvc.Pager.PagedList<TEntity> GetList(SF.Framework.Mvc.Search.Model.QueryModel model,
                                                                    string orderByField,
                                                                    string orderSort,
                                                                    int PageIndex = 1,
                                                                    int PageSize = 24);

        IList<TEntity> GetList(SF.Framework.Mvc.Search.Model.QueryModel model);

        Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                 SF.Framework.Mvc.Search.Model.QueryModel model,
                                                                Dictionary<string, bool> paramNames,
                                                                 int PageIndex = 1,
                                                                 int PageSize = 24);
        Mvc.Pager.PagedList<object> GetList(Expression<Func<TEntity, bool>> conditionExpression,
                                                                  Dictionary<string, bool> paramNames,
                                                                  int PageIndex = 1,
                                                                  int PageSize = 24);
        Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                                   Expression<Func<TEntity, bool>> conditionExpression,
                                                                   Dictionary<string, bool> paramNames,
                                                                   int PageIndex = 1,
                                                                   int PageSize = 24);
        Mvc.Pager.PagedList<object> GetList(Func<IQueryable<TEntity>, List<object>> selector,
                                                            Core.Specifications.ISpecification<TEntity> spec,
                                                               Dictionary<string, bool> paramNames,
                                                               int PageIndex = 1,
                                                               int PageSize = 24);
        void GetList(Dictionary<string, bool> paramNames);
        #endregion
    }
}
