using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VintageBookshelf.Data.Context;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly VintageBookshelfContext Db;
        private readonly DbSet<TEntity> _dbSet;

        protected Repository(VintageBookshelfContext db)
        {
            Db = db;
            _dbSet = db.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetById(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        
        public virtual async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChanges();
        }
        
        public virtual async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChanges();
        }
        
        public virtual async Task Remove(long id)
        {
            var entity =  await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            Db.Dispose();
        }
    }
}