using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace LibraryApp.Models
{
    public class AddBookView
    {
        public string Title { get; set; }
        public string Publishing { get; set; }
        public DateTime DateOfPublish { get; set; }
        public int AviableCount { get; set; }
        public string Text { get; set; }
        public string PictureTitle { get; set; }
        public HttpPostedFileBase uploadImage { get; set; }
        public string AuthorFullName { get; set; }
        public string SubjectTitle { get; set; }
     

    }
}