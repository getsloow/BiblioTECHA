using BiblioTecha.Areas.Identity.Data;
using BiblioTecha.Controllers;

namespace BiblioTecha.Models
{
    public class BookDetailsViewModel
    {
        public BookModel Book { get; set; }
        public AuthorModel Author { get; set; }
        public FileModel File { get; set; }
    }
}
