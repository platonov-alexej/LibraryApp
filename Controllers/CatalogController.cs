using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryApp.Controllers
{
    public class CatalogController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // GET: Catalog
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        //[Authorize(Roles = "librarian")]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public string AddBook(Book book, string author, string subject)
        {

            Author authorLocal = db.Authors.FirstOrDefault(p => p.FullName == author);
            if (authorLocal == null)
            {
                db.Authors.Add(new Author { FullName = author });
                db.SaveChanges();
            }
            book.AuthorId = db.Authors.FirstOrDefault(p => p.FullName == author).Id;

            Subject subjectLocal = db.Subjects.FirstOrDefault(p => p.Title == subject);
            if (subjectLocal == null)
            {
                db.Subjects.Add(new Subject { Title = subject });
                db.SaveChanges();
            }
            book.SubjectId = db.Subjects.FirstOrDefault(p => p.Title == subject).Id;

            db.Books.Add(book);
            db.SaveChanges();


            return $"Книга {book.Title} добавлена";
        }

        [HttpGet]
        //[Authorize(Roles = "reader")]
        public ActionResult TakeBook(int id)
        {
            Book book = db.Books.FirstOrDefault(b => b.Id == id);
            ViewBag.book = book;
            ViewBag.author = db.Authors.FirstOrDefault(a => a.Id == book.AuthorId).FullName;
            ViewBag.subject = db.Subjects.FirstOrDefault(s => s.Id == book.SubjectId).Title;
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "reader")]
        public string TakeBook(int Id, string Title)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            BookGiving bg = new BookGiving
            {
                Book = db.Books.Find(Id),
                DateGiving = DateTime.Now,
                IsReturned = false,
                ApplicationUser = userManager.FindByEmail("librarian@gmail.com")
            };
            db.BookGivings.Add(bg);
            db.SaveChanges();

            return $"Книга  добавлена";
        }



    }
}