using Microsoft.AspNetCore.Mvc;

namespace BiblioTecha.Models
{
    public class BookModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool Available { get; set; }

        public string Genre { get; set; }
        public string ImageLink { get; set; }


    }
}
