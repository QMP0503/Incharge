using Incharge.Data;
using Incharge.Models;
using Incharge.Repository.IRepository;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZstdSharp.Unsafe;

namespace Incharge.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly InchargeContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILog _logger;
        public Repository(InchargeContext context, ILog log)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = log;
        }
        public void Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
            }
            catch (Exception ex) {_logger.Error(ex); }
        }
        public void Update(T entity)
        {
            try
            {
                _context.Update(entity);
            }
            catch (Exception ex) {_logger.Error(ex); }
        }
        public void Delete(T entity)
        {
            try
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            }
            catch (Exception ex) {_logger.Error(ex); }
        }
        public void Save() //finish all operations before saving
        {
            _context.SaveChanges();
        }

    }
}
