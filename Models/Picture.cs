using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Picture
    {
        [Key]
        public int PictureId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ICollection<Book> Book { get; set; }
        public Picture()
        {
            Book = new List<Book>();
        }
    }
}