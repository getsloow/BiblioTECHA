using Microsoft.AspNetCore.Mvc;

namespace BiblioTecha.Models
{
    public class BookModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Available { get; set; }
        public int ReadingDays { get; set; }
        public string Genre { get; set; }
        public string ImageLink { get; set; }
        public int AuthorId { get; set; }
        public AuthorModel? Author { get; set; }
        public FileModel? File { get; set; }


    }




}
