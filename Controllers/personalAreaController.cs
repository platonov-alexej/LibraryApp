using LibraryApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace LibraryApp.Controllers
{
    public class BookForView
    {
        public Book Book { get; set; }
        public string  Picture { get; set; }
    }
    public class personalAreaController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Читатель, Магистр, Библиотекарь")]
        public ActionResult Index()
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            string userRole = userManager.GetRoles(User.Identity.GetUserId()).ToArray()[0];
            ViewBag.Role = userRole;

            string userId = User.Identity.GetUserId();

            var givings = db.BookGivings.Where(g => g.ApplicationUserId == userId && g.IsReturned == false).ToList();

            List<Book> books = new List<Book>();
            foreach(var giving in givings)
                books.Add(db.Books.Include("Author")
                                    .Include("Subject")
                     .FirstOrDefault(x => x.Id == giving.BookId));

            List < BookForView> pairs = new List<BookForView>();
            foreach(var book in books)
            {
                pairs.Add(new BookForView { Book = book, Picture = Convert.ToBase64String(Tools.Decompress(db.Pictures.Find(book.PictureId).Image)) });
            }
            ViewBag.Items = pairs;

            
            return userRole == "Библиотекарь" ? View("IndexLibrarian") : View();
        }

    public ActionResult ViewAllUsers()
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var users = userManager.Users;
            int usersLength = users.ToArray().Length;

            IList<string>[] roles = new IList<string>[usersLength];

            for (int i = 0; i < usersLength; i++)
            {
                roles[i] = userManager.GetRoles(users.ToArray()[i].Id);

            }
            ViewBag.Users = users;
            ViewBag.Roles = roles;
            return View();
        }

        public class BooksAtUsers
        {
            public string Book { get; set; }
            public string User { get; set; }
            public string Date { get; set; }
        }
        public ActionResult ViewBooksAtUsers()
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var booksAtUsers = db.BookGivings.Where(bg => !bg.IsReturned).ToArray();
            List<BooksAtUsers> bau = new List<BooksAtUsers>();
            foreach(var b in booksAtUsers)
            {
                bau.Add(new BooksAtUsers {
                    Book = db.Books.FirstOrDefault(x => x.Id == b.BookId).Title,
                    User = userManager.FindById(b.ApplicationUserId).Email,
                    Date = b.DateGiving.ToString("D")
                });
            }
            ViewBag.BookGivings = bau;
            return View();
        }

    }
}