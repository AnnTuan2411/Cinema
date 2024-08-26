using Cinema.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private CinemaContext db = null;

        private DbSet<T> table = null;

        public GenericRepository()
        {
            this.db = new CinemaContext();
            table = db.Set<T>();
        }
        public GenericRepository(CinemaContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            db.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}