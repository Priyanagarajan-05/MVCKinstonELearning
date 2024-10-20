using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using coursemvc.Data;
using coursemvc.Models;
using System.Linq;

namespace coursemvc.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve professor's details from session
            var professorName = HttpContext.Session.GetString("UserName");
            var professorEmail = HttpContext.Session.GetString("UserEmail");

            ViewBag.ProfessorName = professorName;
            ViewBag.ProfessorEmail = professorEmail;

            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the session and redirect to the login page
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        // GET: Professor/CreateCourse
        [HttpGet]
        public IActionResult CreateCourse()
        {
            // Get the logged-in professor's email from the session
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User"); // If not logged in, redirect to login
            }

            // Find the professor by email
            var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");
            if (professor == null)
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            // Create a new Course model instance with default values
            var model = new Course
            {
                IsAccount = false, // Default to false
                ProfessorId = professor.UserId // Automatically assign the ProfessorId
            };

            return View(model);
        }

        // POST: Professor/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    course.IsAccount = false; // Explicitly set IsAccount to false
                    _context.Courses.Add(course); // Add the new course to the database
                    _context.SaveChanges(); // Save changes to the database

                    TempData["StatusMessage"] = "Course created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the error if necessary, and show the error message
                    ModelState.AddModelError("", "Unable to save course. " + ex.Message);
                }
            }

            // If the model state is invalid, return the form with errors
            return View(course);
        }

        // GET: Professor/MyCourses
        [HttpGet]
        public IActionResult MyCourses()
        {
            // Get the logged-in professor's email from the session
            var email = HttpContext.Session.GetString("UserEmail");
            var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");

            if (professor == null)
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            // Retrieve the list of courses for this professor from the database
            var courses = _context.Courses.Where(c => c.ProfessorId == professor.UserId).ToList();

            return View(courses);
        }
    }
}
