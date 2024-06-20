using Application.Interfaces.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>,
        IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "",
        int? topCount = null
        )
        where TEntity : class
        {
            try
            {
                IQueryable<TEntity> query = _dbContext.Set<TEntity>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) =>
                    current.Include(includeProperty));

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }
                if (topCount.HasValue)
                {
                    query = query.Take(topCount.Value);
                }
                return await query.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<IEnumerable<TProperty>> GetTProperty<TEntity, TProperty>(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Expression<Func<TEntity, TProperty>> selectProperty = null,
        string includeProperties = "",
        int? topCount = null)
        where TEntity : class
        {
            try
            {
                IQueryable<TEntity> query = _dbContext.Set<TEntity>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) =>
                        current.Include(includeProperty));

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (topCount.HasValue)
                {
                    query = query.Take(topCount.Value);
                }

                return await query.Cast<TProperty>().ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<TEntity> GetById<TEntity>(object id) where TEntity : class
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filter);
        }

        public async Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            return filter != null && await _dbContext.Set<TEntity>().AnyAsync(filter);
        }

        public async Task<int> Insert<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null) throw new ArgumentNullException("Entity");

            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            var ret = 0;
            var key = typeof(TEntity).GetProperties().FirstOrDefault(p =>
                p.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute)));

            if (key != null)
            {
                var keyType = key.PropertyType;

                if (keyType == typeof(int))
                {
                    ret = (int)key.GetValue(entity, null)!;
                }
                else if (keyType == typeof(long))
                {
                    ret = Convert.ToInt32(key.GetValue(entity, null));
                }
            }

            return ret;
        }

        public async Task AddMultipleEntity<TEntity>(IEnumerable<TEntity> entityList) where TEntity : class
        {

            if (entityList == null) throw new ArgumentNullException("Entity");

            await _dbContext.Set<TEntity>().AddRangeAsync(entityList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update<TEntity>(TEntity entityToUpdate) where TEntity : class
        {
            if (entityToUpdate == null) throw new ArgumentNullException("Entity");

            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateMultipleEntity<TEntity>(IEnumerable<TEntity> entityList) where TEntity : class
        {

            if (entityList == null) throw new ArgumentNullException("Entity");

            _dbContext.Entry(entityList).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete<TEntity>(object id) where TEntity : class
        {

            var entityToDelete = await _dbContext.Set<TEntity>().FindAsync(id);

            if (entityToDelete == null) throw new ArgumentNullException("Entity");

            _dbContext.Set<TEntity>().Remove(entityToDelete);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMultipleEntity<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {

            var query = _dbContext.Set<TEntity>().Where(filter);

            _dbContext.Set<TEntity>().RemoveRange(query);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete<TEntity>(TEntity entityToDelete) where TEntity : class
        {
            if (entityToDelete == null) throw new ArgumentNullException("Entity");

            _dbContext.Set<TEntity>().Remove(entityToDelete);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveMultipleEntity<TEntity>(IEnumerable<TEntity> removeEntityList) where TEntity : class
        {
            if (removeEntityList == null) throw new ArgumentNullException("Entity");

            _dbContext.Set<TEntity>().RemoveRange(removeEntityList);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.CountAsync();
        }
    }
}
