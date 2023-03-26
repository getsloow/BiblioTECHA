using Microsoft.AspNetCore.Mvc;

namespace BiblioTecha.Models
{
    public class BookModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Available { get; set; }
        public int ReadingDays { get; set; }
        public string Genre { get; set; }
        public string ImageLink { get; set; }

    }

    public class ReservationModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int BookId { get; set; }
        public BookModel Book { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
    }


}
