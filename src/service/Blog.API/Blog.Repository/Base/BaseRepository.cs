#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.IRepository.Base;
using Blog.Model.Base;
using Blog.Model.Mapping;
using Blog.Model.ParameterModel;
using Blog.Model.ParameterModel.Base;
using Blog.Model.Resources;
using Blog.Model.ViewModel;
using Blog.Repository.Sugar;
using SqlSugar;

#endregion

namespace Blog.Repository.Base
{
    public  class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel, new()
    {
        public DbContext Context { get; set; }
        internal SqlSugarClient Db { get; private set; }
        internal SimpleClient<TEntity> EntityDB { get; private set; }

        internal IPropertyMappingContainer MappingContainer { get; set; }

        public BaseRepository()
        {
            DbContext.Init(BaseDBConfig.ConnectionString);
            Context = DbContext.GetDbContext();
            Db = Context.Db;
            EntityDB = Context.GetEntityDB<TEntity>(Db);
        }

        // TODO: 想办法找个更好的方式DI
        public void SetMapperContainer(IPropertyMappingContainer container)
        {
            this.MappingContainer = container;
        }

        public async Task<TEntity> QueryByID(object objId)
        {
            return await QueryByID(objId, false);
        }

        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryByID(object objId, bool blnUseCache)
        {
            return await Task.FromResult(Db.Queryable<TEntity>().WithCacheIF(blnUseCache).InSingle(objId));
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            return await Task.FromResult(Db.Queryable<TEntity>().In(lstIds).ToList());
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<int> Add(TEntity entity)
        {
            var i = await Task.FromResult(Db.Insertable(entity).ExecuteReturnBigIdentity());
            return (int)i;
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            //这种方式会以主键为条件
            var i = await Task.FromResult(Db.Updateable(entity).ExecuteCommand());
            return i > 0;
        }

        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            return await Task.FromResult(Db.Updateable(entity).Where(strWhere).ExecuteCommand() > 0);
        }

        public async Task<bool> Update(string strSql, SugarParameter[] parameters = null)
        {
            return await Task.FromResult(Db.Ado.ExecuteCommand(strSql, parameters) > 0);
        }

        public async Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
        {
            var up = await Task.FromResult(Db.Updateable(entity));
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = await Task.FromResult(up.IgnoreColumns(lstIgnoreColumns.Contains));
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = await Task.FromResult(up.UpdateColumns(lstColumns.Contains));
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = await Task.FromResult(up.Where(strWhere));
            }
            return await Task.FromResult(up.ExecuteCommand()) > 0;
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            var i = await Task.FromResult(Db.Deleteable(entity).ExecuteCommand());
            return i > 0;
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            var i = await Task.FromResult(Db.Deleteable<TEntity>(id).ExecuteCommand());
            return i > 0;
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            var i = await Task.FromResult(Db.Deleteable<TEntity>().In(ids).ExecuteCommand());
            return i > 0;
        }



        /// <summary>
        /// 功能描述:查询所有数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            return await Task.FromResult(EntityDB.GetList());
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere)
        {
            return await Task.FromResult(Db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Task.FromResult(EntityDB.GetList(whereExpression));
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await Task.FromResult(Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToList());
        }
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await Task.FromResult(Db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList());
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await Task.FromResult(Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds)
        {
            return await Task.FromResult(Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList());
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            string strWhere,
            int intTop,
            string strOrderByFileds)
        {
            return await Task.FromResult(Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToList());
        }

        public async Task<PaginatedList<TEntity>> QueryPage<TMapping>(Expression<Func<TEntity, bool>> whereExpression,
            QueryParameters parameters, string select) where TMapping : BaseViewModel {
            var query = Db.Queryable<TEntity>();
            query = query.ApplySort(parameters.OrderBy, MappingContainer.Resolve<TMapping, TEntity>());

            var count = query.Count();
            var data = query.Skip(parameters.PageIndex * parameters.PageSize).Take(parameters.PageSize).Select(select).ToList();

            return await Task.FromResult(new PaginatedList<TEntity>(parameters.PageIndex, parameters.PageSize, count, data));
        }
    }
}
