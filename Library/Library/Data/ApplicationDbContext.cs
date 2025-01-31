using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } 
      
        public ApplicationDbContext() { }

        // DbSet properties for entities
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }

    // Entity classes
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int AvailableCopies { get; set; }

        public bool? IsAvailable => AvailableCopies > 0 ? true : null;
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
    }
}
