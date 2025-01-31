using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BooksApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BooksApi
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _context.Books.ToList(); // Fetch all books
            return Ok(books);
        }

        // GET: api/BooksApi/1
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST: api/BooksApi
        [HttpPost]
        public IActionResult CreateBook([FromBody] Book newBook)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Books.Add(newBook); // Add the book to the database
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        // PUT: api/BooksApi/1
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            book.Title = updatedBook.Title; 
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/BooksApi/1
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            _context.Books.Remove(book); 
            _context.SaveChanges();

            return NoContent();
        }
    }
}
