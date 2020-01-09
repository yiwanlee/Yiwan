using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryDemo.Data
{
    public class MyDbContext : DbContext
    {
        //EF从配置文件中寻找name=BookStore的数据库链接。
        //如果不指定: base("BookStore")，则寻找name=BookStoreDbContext的数据库连接
        public MyDbContext()
            : base("BookStore")
        {

        }
    }
}
