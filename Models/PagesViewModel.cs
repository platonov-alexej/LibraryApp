using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryApp.Models
{
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int TotalPages { get; set; }// всего страниц
    }

    public class PagesViewModel
    {
        public Book Book { get; set; }
        public string Picture { get; set; }
        public string Text { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}