using System;

namespace Cinema.Repository_2
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById (int id);
        IEnumerable<T> GetAll ();
        void Add(T entity);
        void Update (T entity);
        void Delete (T entity);
    }
}
