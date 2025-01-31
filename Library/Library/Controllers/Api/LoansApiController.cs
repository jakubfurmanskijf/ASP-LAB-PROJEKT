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
    public class LoansApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoansApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LoansApi
        [HttpGet]
        public IActionResult GetAllLoans()
        {
            var loans = _context.Loans.ToList(); 
            return Ok(loans);
        }

        // GET: api/LoansApi/1
        [HttpGet("{id}")]
        public IActionResult GetLoanById(int id)
        {
            var loan = _context.Loans.FirstOrDefault(l => l.Id == id);
            if (loan == null) return NotFound();
            return Ok(loan);
        }

        // POST: api/LoansApi
        [HttpPost]
        public IActionResult CreateLoan([FromBody] Loan newLoan)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Loans.Add(newLoan); // Add the loan to the database
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetLoanById), new { id = newLoan.Id }, newLoan);
        }

        // PUT: api/LoansApi/1
        [HttpPut("{id}")]
        public IActionResult UpdateLoan(int id, [FromBody] Loan updatedLoan)
        {
            var loan = _context.Loans.FirstOrDefault(l => l.Id == id);
            if (loan == null) return NotFound();

            loan.UserId = updatedLoan.UserId; 
            loan.BookId = updatedLoan.BookId; 
            loan.LoanDate = updatedLoan.LoanDate; 
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/LoansApi/1
        [HttpDelete("{id}")]
        public IActionResult DeleteLoan(int id)
        {
            var loan = _context.Loans.FirstOrDefault(l => l.Id == id);
            if (loan == null) return NotFound();

            _context.Loans.Remove(loan); // Remove the loan
            _context.SaveChanges();

            return NoContent();
        }
    }
}
