using Hospital.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Implementation
{
    public class UnitOfWork:IUnitOfWork,IDisposable
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
                _context = context;
        }
        #endregion

        #region Dispose
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  
        }
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;   
        }
        #endregion

        #region Save

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            IGenericRepository<T> repository = new GenericRepository<T>(_context);
            return repository;
        }
        public void Save()
        {
            _context.SaveChanges(); 
        }

    
        #endregion
    }
}
