using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public ICollection<Book> Books { get; set; }
        public Author()
        {
            Books = new List<Book>();
        }
    }
}