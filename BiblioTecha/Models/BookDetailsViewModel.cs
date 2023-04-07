using BiblioTecha.Areas.Identity.Data;

namespace BiblioTecha.Models
{
    public class BookDetailsViewModel
    {
        public BookModel Book { get; set; }
        public AuthorModel Author { get; set; }
    }
}
