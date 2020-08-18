using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Page
    {
        [Key]
        public int PageId { get; set; }
        public byte[] BytesText { get; set; }
        public int Number { get; set; }


        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}