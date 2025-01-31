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
    public class UsersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UsersApi
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList(); // Fetch users from the database
            return Ok(users);
        }

        // GET: api/UsersApi/1
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/UsersApi
        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Users.Add(newUser); // Add the user to the database
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // PUT: api/UsersApi/1
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name; // Update the name
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/UsersApi/1
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            _context.Users.Remove(user); // Remove the user from the database
            _context.SaveChanges();

            return NoContent();
        }
    }
}
