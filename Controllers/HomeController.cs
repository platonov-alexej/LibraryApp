﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO.Compression;

namespace LibraryApp.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            //Subject subj1 = new Subject { Title = "Потерянное поколение" };
            //Subject subj2 = new Subject { Title = "Детектив" };
            //db.Subjects.Add(subj2);
            //db.Subjects.Add(subj1);
            //Book b1 = new Book { Title = "Черный oбелиск", AuthorId = 1, SubjectId = 1, Publishing = "Радуга", DateOfPublish = Convert.ToDateTime("01-01-2000"), AviableCount = 5 };
            //db.Books.Add(b1);
            //db.SaveChanges();


            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            ////// создаем две роли
            //var role1 = new IdentityRole { Name = "Библиотекарь" };
            //var role2 = new IdentityRole { Name = "Магистр" };
            //var role3 = new IdentityRole { Name = "Читатель" };

            ////// добавляем роли в бд
            //roleManager.Create(role1);
            //roleManager.Create(role2);
            //roleManager.Create(role3);

            //Book b1 = new Book { Title = "На западном фронте без перемен", AuthorId = 1, SubjectId = 2, Publishing = "Радуга", DateOfPublish = Convert.ToDateTime("01-01-2010"), AviableCount = 3 };
            //db.Books.Add(b1);
            //db.SaveChanges();

            IEnumerable<Author> authors = db.Authors;
            ViewBag.Authors = authors;
            IEnumerable<Subject> subjects = db.Subjects;
            ViewBag.Subjects = subjects;

            var books = db.Books
                .Include("Author")
                .Include("Subject")
                .ToList();
            ViewBag.Books = books;

            List<BookForView> pairs = new List<BookForView>();
            foreach (var book in books)
            {
                pairs.Add(new BookForView { Book = book, Picture = Convert.ToBase64String(Tools.Decompress(db.Pictures.Find(book.PictureId).Image)) });
            }
            ViewBag.Items = pairs;


            //var pictures = db.Pictures;
            //ViewBag.Pictures = pictures;
            //ViewBag.role = User.IsInRole("reader");

            //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            //var roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(db));

            //var libr = User.Identity.GetUserId();
            //userManager.AddToRole(userManager.FindByEmail("librarian@gmail.com").Id, "librarian"); ;


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}