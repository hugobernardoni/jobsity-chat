using JobSity.Model.Models;
using JobSity.Repositories.Abstract;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JobSity.DAO;

namespace JobSity.Repositories.Repositories
{
    public class EntityBaseRepositoryAsync<T> : IEntityBaseRepositoryAsync<T> where T : EntityBase, new()
    {

        private JobSityContext _context;

        #region Constructor
        public EntityBaseRepositoryAsync(JobSityContext context)
        {
            _context = context;
        }

        #endregion
        public async virtual Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async virtual Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }
        public async virtual Task<List<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetSingleAsync(Guid id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingleAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate);
        }

        public async virtual Task AddAsync(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public void DeleteById(Guid id)
        {
            var objectToDelete = new T { Id = id };
            _context.Attach<T>(objectToDelete);
            _context.Remove(objectToDelete);
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            foreach (var entity in _context.Set<T>().Where(predicate))
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public async virtual Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
