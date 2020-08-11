using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryApp.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Book> Books { get; set; }
        public Subject()
        {
            Books = new List<Book>();
        }
    }
}