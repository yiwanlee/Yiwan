using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryDemo.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        void Save();
        bool IsDisposed { get; }
    }
}
