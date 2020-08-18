using System;
using System.Linq;
using System.Web.Mvc;
using LibraryApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;

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
        [Authorize(Roles = "Библиотекарь")]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Библиотекарь")]
        public ActionResult AddBook(AddBookView model)
        {
            Picture pic = new Picture();

            byte[] imageData = null;
            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(model.uploadImage.InputStream))
            {
                imageData = binaryReader.ReadBytes(model.uploadImage.ContentLength);
            }
            // установка массива байтов

            pic.Name = model.PictureTitle;
            pic.Image = Tools.Compress(imageData);

            db.Pictures.Add(pic);
            db.SaveChanges();

            int pageSize = 1000;
            int pagesCount = model.Text.Length % pageSize == 0 ? model.Text.Length / pageSize : model.Text.Length / pageSize + 1;

            Book book = new Book
            {
                Title = model.Title,
                Publishing = model.Publishing,
                DateOfPublish = model.DateOfPublish,
                AviableCount = model.AviableCount,
                TotalPages = pagesCount
            };

            Author authorLocal = db.Authors.FirstOrDefault(p => p.FullName == model.AuthorFullName);
            if (authorLocal == null)
            {
                db.Authors.Add(new Author { FullName = model.AuthorFullName });
                db.SaveChanges();
            }

            book.AuthorId = db.Authors.FirstOrDefault(p => p.FullName == model.AuthorFullName).Id;

            Subject subjectLocal = db.Subjects.FirstOrDefault(p => p.Title == model.SubjectTitle);
            if (subjectLocal == null)
            {
                db.Subjects.Add(new Subject { Title = model.SubjectTitle });
                db.SaveChanges();
            }
            book.SubjectId = db.Subjects.FirstOrDefault(p => p.Title == model.SubjectTitle).Id;

            book.PictureId = pic.PictureId;

            db.Books.Add(book);
            db.SaveChanges();

            
            char[] tmpArr = model.Text.ToCharArray();
            for (int i = 1; i <= pagesCount; i++)
            {
                Page page = new Page
                {
                    Number = i,
                    BookId = book.Id,
                };

                string s = String.Join("", tmpArr.Skip((i - 1) * pageSize).Take(pageSize));
                page.BytesText = Tools.Compress(Encoding.UTF8.GetBytes(s));
                db.Pages.Add(page);
            }
            db.SaveChanges();


            return RedirectToAction("../Home/Index"); ;
        }

        [HttpGet]
        [Authorize(Roles = "Читатель, Магистр, Библиотекарь")]
        public ActionResult TakeBook(int id)
        {

            Book book = db.Books.Include("Author").Include("Subject").FirstOrDefault(b => b.Id == id);
            string currentUserId = User.Identity.GetUserId();
            var booksAtUser = db
                .BookGivings
                .Where(m => (m.ApplicationUserId == currentUserId && m.IsReturned == false)).ToList();

            bool isPotentialDublicate = false;

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            if (!userManager.IsInRole(User.Identity.GetUserId(), "Библиотекарь") && !userManager.IsInRole(User.Identity.GetUserId(), "Магистр"))
            {
                foreach (var bookAtUser in booksAtUser)
                    if (bookAtUser.BookId == id)
                        isPotentialDublicate = true;
            }

            int maxPossibleCountBook = userManager.IsInRole(User.Identity.GetUserId(), "Читатель") ? 3 : 20;

            if (book.AviableCount > 0 && booksAtUser.Count < maxPossibleCountBook && !isPotentialDublicate)
            {
                BookForView pair = new BookForView();
                pair.Book = book;
                pair.Picture = Convert.ToBase64String(Tools.Decompress(db.Pictures.Find(book.PictureId).Image));
                ViewBag.Item = pair;
                return View();
            }
            else
            {
                return View("NoBooksAviable");
            }

        }

        [HttpPost]
        [Authorize(Roles = "Читатель, Магистр, Библиотекарь")]
        public ActionResult TakeBook(int id, string Title)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            Book book = db.Books.Find(id);
            BookGiving bg = new BookGiving
            {
                Book = book,
                DateGiving = DateTime.Now,
                IsReturned = false,
                ApplicationUser = userManager.FindById(User.Identity.GetUserId())
            };
            db.BookGivings.Add(bg);
            book.AviableCount--;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("../personalArea/Index"); ;
        }

        [HttpGet]
        [Authorize(Roles = "Читатель, Магистр, Библиотекарь")]
        public ActionResult ReturnBook(int id)
        {

            Book book = db.Books.Include("Author").Include("Subject").FirstOrDefault(b => b.Id == id);

            BookForView pair = new BookForView();
            pair.Book = book;
            pair.Picture = Convert.ToBase64String(Tools.Decompress(db.Pictures.Find(book.PictureId).Image));
            ViewBag.Item = pair;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Читатель, Магистр, Библиотекарь")]
        public ActionResult ReturnBook(int id, string Title)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            string userId = User.Identity.GetUserId();
            BookGiving bg = db
                .BookGivings
                .FirstOrDefault(m => m.BookId == id && m.ApplicationUserId == userId && m.IsReturned == false);
            bg.IsReturned = true;
            db.Entry(bg).State = EntityState.Modified;

            Book book = db.Books.Find(id);
            book.AviableCount++;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();

            if (userManager.IsInRole(User.Identity.GetUserId(), "Читатель"))
            {
                var givings = db.BookGivings.Where(g => g.ApplicationUserId == userId && g.IsReturned == true).ToList();
                if (givings.Count() >= 10)
                {
                    userManager.RemoveFromRole(userId, "Читатель");
                    userManager.AddToRole(userId, "Магистр");
                    db.SaveChanges();

                }
            }

            return RedirectToAction("../personalArea/Index");
        }

        [HttpGet]
        public ActionResult ViewBook(int id, int page = 1)
        {
            int bookId = id;
            int pageNumber = page;
            PagesViewModel pageView = new PagesViewModel();

            Book book = db.Books.Include("Author").Include("Subject").FirstOrDefault(b => b.Id == bookId);
            pageView.Book = book;

            pageView.Picture = Convert.ToBase64String(Tools.Decompress(db.Pictures.Find(book.PictureId).Image));

            var pages = db.Pages.FirstOrDefault(p => p.Number == pageNumber && p.BookId == bookId);
            PageInfo pageInfo = new PageInfo();
            if (pages != null)
            {
                byte[] byteText = Tools.Decompress(pages.BytesText);
                char[] utf8Chars = new char[Encoding.UTF8.GetCharCount(byteText, 0, byteText.Length)];
                Encoding.UTF8.GetChars(byteText, 0, byteText.Length, utf8Chars, 0);
                pageView.Text = new string(utf8Chars);

                pageInfo.PageNumber = pageNumber;
                pageInfo.TotalPages = book.TotalPages;
            }
  
            pageView.PageInfo = pageInfo;

            return View(pageView);
        }
    }
}