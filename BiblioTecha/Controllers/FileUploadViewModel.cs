namespace BiblioTecha.Controllers
{
    public class FileUploadViewModel
    {
        public IFormFile File { get; set; }
        public int? BookId { get; set; }
        public string UploadedBy { get; set; }
    }
}
