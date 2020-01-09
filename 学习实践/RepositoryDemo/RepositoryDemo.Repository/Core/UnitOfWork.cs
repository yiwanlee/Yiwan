using RepositoryDemo.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryDemo.Repository.Core
{
    /// <summary>
    /// 用于对数据的保存操作
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _context;
        public DbContext Context
        {
            get { return _context; }
        }

        public event EventHandler Disposed;

        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            Dispose(true);
        }
        public virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !IsDisposed)
                {
                    _context.Dispose();
                    var evt = Disposed;
                    if (evt != null) evt(this, EventArgs.Empty);
                    Disposed = null;
                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        public UnitOfWork(MyDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
