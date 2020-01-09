using RepositoryDemo.Entity;
using RepositoryDemo.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryDemo.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        //其他业务操作，在控制器中一般使用的是该接口
        IList<Book> GetAllBooks();
    }
}
