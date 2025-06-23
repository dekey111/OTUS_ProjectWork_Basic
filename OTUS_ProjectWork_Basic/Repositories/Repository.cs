using Microsoft.EntityFrameworkCore;
using OTUS_ProjectWork_Basic.DataBase;
using OTUS_ProjectWork_Basic.Interfaces;

namespace OTUS_ProjectWork_Basic.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CloudReaderContext _context;

        public Repository(CloudReaderContext context) => _context = context;

        public void Add(T entity) => _context.Set<T>().Add(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Remove(T entity) => _context.Set<T>().Remove(entity);
        public T GetById(int id) => _context.Set<T>().Find(id);
        public IEnumerable<T> GetAll() => _context.Set<T>().ToList();
    }
}
