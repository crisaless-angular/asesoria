using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Web.Business.Interfaces;
using Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.Business.Genericrepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AsesoriaContext _context;
        private readonly Microsoft.EntityFrameworkCore.DbSet<TEntity> _dbSet;

        public GenericRepository(AsesoriaContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<TEntity>();
        }

        public List<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public TEntity GetEntity(int id)
        {
            return _dbSet.Find(id);
        }
        public void Add(TEntity Object)
        {
            _dbSet.Add(Object);
        }

        public void Update(TEntity Object)
        {
            _dbSet.Update(Object);
        }

        public void Delete(TEntity Object)
        {
            _dbSet.Remove(Object);
        }

    }
}
