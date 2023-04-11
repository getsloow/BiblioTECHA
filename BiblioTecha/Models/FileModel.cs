namespace BiblioTecha.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int? BookId { get; set; }
        public virtual BookModel Book { get; set; }
    }
}
