using Cinema.Models;

namespace Cinema.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void Update(T Obj);
        void Delete(object id);
        void Save();
    }
}
