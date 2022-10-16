using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Business.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();
        TEntity GetEntity(int id);
        void Add(TEntity Object);
        void Update(TEntity Object);
        void Delete(TEntity Object);
    }
}