using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IGenericRepository
    {
        Task<IEnumerable<TProperty>> GetTProperty<TEntity, TProperty>(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Expression<Func<TEntity, TProperty>> selectProperty = null,
        string includeProperties = "",
        int? topCount = null)
        where TEntity : class;

        #region Get Item
        Task<TEntity> GetById<TEntity>(object id) where TEntity : class;

        Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;
        #endregion

        #region Data Insertion
        Task<int> Insert<TEntity>(TEntity entity) where TEntity : class;

        Task AddMultipleEntity<TEntity>(IEnumerable<TEntity> entityList) where TEntity : class;
        #endregion

        #region Data Updation

        Task Update<TEntity>(TEntity entityToUpdate) where TEntity : class;

        Task UpdateMultipleEntity<TEntity>(IEnumerable<TEntity> entityList) where TEntity : class;

        #endregion

        #region Data Deletion

        Task Delete<TEntity>(object id) where TEntity : class;

        Task DeleteMultipleEntity<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;

        Task Delete<TEntity>(TEntity entityToDelete) where TEntity : class;

        Task RemoveMultipleEntity<TEntity>(IEnumerable<TEntity> removeEntityList) where TEntity : class;

    }
}
