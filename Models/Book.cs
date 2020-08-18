using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Publishing { get; set; }
        public DateTime DateOfPublish { get; set; }
        public int AviableCount { get; set; }
        public int TotalPages { get; set; }

        [ForeignKey("Picture")]
        public int? PictureId { get; set; }
        public Picture Picture { get; set; }

        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        public Author Author { get; set; }


        [ForeignKey("Subject")]
        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<BookGiving> BookGivings { get; set; }
        public ICollection<Page> Pages { get; set; }
        public Book()
        {
            BookGivings = new List<BookGiving>();
            Pages = new List<Page>();
        }
    }
}



//<form action = "/Catalog/TakeBook" method="post">
//    <div class="card" style="width: 18rem;">
//        <div>
//            <img src = "@book.pathImg" width="250" class="card-img-top" alt="...">
//        </div>
//        <div class="card-body">

//            <h3 class="card-title">@book.Title</h3>
//            <h4 class="card-text">@ViewBag.author</h4>
//            <p class="card-text">@ViewBag.subject</p>


//            <button name = "action" value="add" type="submit">Добавить</button>


//        </div>
//    </div>
//</form>
