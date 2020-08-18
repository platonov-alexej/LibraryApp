using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class BookGiving
    {
        [Key]
        public int BooksGivingId { get; set; }

        [ForeignKey("Book")]
        public int? BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime DateGiving { get; set; }
        public bool IsReturned { get; set; }
    }
}