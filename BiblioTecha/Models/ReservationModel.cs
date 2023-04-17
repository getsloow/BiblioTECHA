namespace BiblioTecha.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int UserScore { get; set; }
        public int BookId { get; set; }
        public int ReservationStatus { get; set; }
        public BookModel Book { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
    }
}
