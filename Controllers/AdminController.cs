using Microsoft.AspNetCore.Mvc;
using coursemvc.Data; // Adjust based on your namespace
using coursemvc.Models; // Ensure you are using the correct namespace for User model
using System.Linq;

namespace coursemvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch users from the database and ensure you check for null
            var users = _context.Users.ToList(); // Ensure Users is the DbSet<User> in your context

            // Ensure users is not null
            if (users == null)
            {
                users = new List<User>(); // Initialize to an empty list if null
            }

            return View(users); // Pass the users list to the view
        }


        [HttpPost]
        public IActionResult AcceptUser([FromBody] UserUpdateRequest request)
        {
            if (request == null || request.UserId <= 0)
            {
                return BadRequest("Invalid request.");
            }

            var user = _context.Users.Find(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.IsActive = true; // Set IsActive to true (1)
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult RejectUser([FromBody] UserUpdateRequest request)
        {
            if (request == null || request.UserId <= 0)
            {
                return BadRequest("Invalid request.");
            }

            var user = _context.Users.Find(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user); // Or set IsActive to false, depending on your logic
            _context.SaveChanges();
            return Ok();
        }
        // GET: Course/ManageCourses
        [HttpGet]
        public IActionResult ManageCourses()
        {
            var courses = _context.Courses.Where(c => !c.IsAccount).ToList(); // Get courses with IsAccount = false
            return View(courses);
        }



        /*
        // POST: Course/AcceptCourse
        [HttpPost]
        public IActionResult AcceptCourse([FromBody] int courseId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            if (course != null)
            {
                course.IsAccount = true; // Set IsAccount to true when accepted
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Course not found");
        }

        // POST: Course/RejectCourse
        [HttpPost]
        public IActionResult RejectCourse([FromBody] int courseId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            if (course != null)
            {
                _context.Courses.Remove(course); // Remove the course if rejected
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Course not found");
        }

        // GET: Course/MyCourses (for Professors)
        [HttpGet]
        public IActionResult MyCourses()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");
            if (professor == null)
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            var courses = _context.Courses.Where(c => c.ProfessorId == professor.UserId && c.IsAccount).ToList(); // Get only approved courses
            return View(courses); // Return the list of approved courses for the logged-in professor
        }
        */
    }
}
