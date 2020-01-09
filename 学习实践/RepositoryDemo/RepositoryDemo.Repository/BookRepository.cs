using RepositoryDemo.Data;
using RepositoryDemo.Entity;
using RepositoryDemo.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryDemo.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        //在执行子类构造函数之前，先执行基类Repository<Book>的构造函数
        public BookRepository(MyDbContext dbcontext)
            : base(dbcontext)
        {
        }

        public IList<Book> GetAllBooks()
        {
            var list = dbContext.Set<Book>();
            return list.OrderBy(x => x.ISBN).ToList();
        }
    }
}
