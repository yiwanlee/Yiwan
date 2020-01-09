using RepositoryDemo.Data;
using RepositoryDemo.Entity;
using RepositoryDemo.Repository;
using RepositoryDemo.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositoryDemo.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository bookRepository;
        private IUnitOfWork unitOfWork;

        public BookController()
        {
            MyDbContext bookStoreDbContext = new MyDbContext();
            bookRepository = new BookRepository(bookStoreDbContext);
            unitOfWork = new UnitOfWork(bookStoreDbContext);
        }

        public ViewResult List()
        {
            IList<Book> listBook = bookRepository.GetAllBooks();
            return View(listBook);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            bookRepository.Insert(book);
            unitOfWork.Save();
            return RedirectToAction("List");
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}