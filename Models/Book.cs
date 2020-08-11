using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Publishing { get; set; }
        public DateTime DateOfPublish { get; set; }
        public int AviableCount { get; set; }
        public string pathImg { get; set; }

        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        public Author Author { get; set; }


        [ForeignKey("Subject")]
        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<BookGiving> BookGivings { get; set; }
        public Book()
        {
            BookGivings = new List<BookGiving>();
        }
    }
}

//<div class="form-group">
//        @Html.Label("Роль пользователя", new { @class = "col-md-2 control-label" })
//        <div class="col-md-10">
//            @Html.DropDownListFor(model => role, ViewBag.Roles as SelectList, new { @class = "form-control" })
//        </div>
//    </div>

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
