using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            // Fetch loans with user names and book titles
            var loans = await _context.Loans
                .Select(loan => new
                {
                    loan.Id,
                    loan.BookId,
                    loan.LoanDate,
                    UserName = _context.Users.Where(u => u.Id == loan.UserId).Select(u => u.Name).FirstOrDefault(),
                    BookName = _context.Books.Where(b => b.Id == loan.BookId).Select(b => b.Title).FirstOrDefault() // Correct logic
                })
                .ToListAsync();

            // Map the query results to LoanViewModel
            var viewModel = loans.Select(l => new LoanViewModel
            {
                Id = l.Id,
                BookId = l.BookId,
                UserName = l.UserName,
                BookName = l.BookName, // This should now be correctly populated
                LoanDate = l.LoanDate
            }).ToList();

            return View(viewModel);
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            ViewBag.Books = new SelectList(_context.Books, "Id", "Title");
            ViewBag.Users = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Loans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loan loan)
        {
            var book = await _context.Books.FindAsync(loan.BookId);

            if (book == null || book.AvailableCopies == 0)
            {
                ModelState.AddModelError("BookId", "This book is not available for loan.");
            }

            if (ModelState.IsValid)
            {
                book.AvailableCopies -= 1; // Decrease available copies
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Books = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewBag.Users = new SelectList(_context.Users, "Id", "Name", loan.UserId);
            return View(loan);
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewBag.Books = new SelectList(_context.Books, "Id", "Title");
            ViewBag.Users = new SelectList(_context.Users, "Id", "Name");
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId,LoanDate")] Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the loan details
            var loan = await _context.Loans.FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            // Fetch the Book Title and User Name explicitly
            var bookTitle = await _context.Books
                .Where(b => b.Id == loan.BookId)
                .Select(b => b.Title)
                .FirstOrDefaultAsync();

            var userName = await _context.Users
                .Where(u => u.Id == loan.UserId)
                .Select(u => u.Name)
                .FirstOrDefaultAsync();

            // Pass Book Title, User Name, and Loan Date Display Name to the view via ViewBag
            ViewBag.BookTitle = bookTitle ?? "Unknown";
            ViewBag.UserName = userName ?? "Unknown";
            ViewBag.LoanDateDisplayName = "Loan Date"; // Custom display name for LoanDate

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Fetch the loan
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound(); // Handle the case where the loan does not exist
            }

            // Find the associated book
            var book = await _context.Books.FindAsync(loan.BookId);
            if (book != null)
            {
                book.AvailableCopies += 1; // Increase available copies
                _context.Books.Update(book); // Mark book as updated
            }

            if (loan != null)
            {
                _context.Loans.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}