namespace Library.Models
{
    public class LoanViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserName { get; set; }
        public string BookName { get; set; }
        public DateTime LoanDate { get; set; }
    }
}