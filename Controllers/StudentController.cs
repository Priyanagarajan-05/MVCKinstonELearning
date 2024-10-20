using Microsoft.AspNetCore.Mvc;
using coursemvc.Data;
using coursemvc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace coursemvc.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get user ID from session
            var userId = HttpContext.Session.GetString("UserId");
            var studentId = _context.Users.FirstOrDefault(u => u.UserId.ToString() == userId && u.Role == "Student")?.UserId;

            if (studentId == null)
            {
                return RedirectToAction("Login", "User"); // Redirect to login if user not found
            }

            // Get all courses and student's enrolled courses
            var allCourses = _context.Courses.Where(c => c.IsAccount).ToList(); // Show only available courses
            var myCourses = _context.StudentCourses.Include(sc => sc.Course)
                .Where(sc => sc.StudentId == studentId).ToList();

            var model = new StudentDashboardViewModel
            {
                AllCourses = allCourses,
                MyCourses = myCourses
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult BuyCourse(int courseId)
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                var studentId = _context.Users.FirstOrDefault(u => u.UserId.ToString() == userId && u.Role == "Student")?.UserId;

                // Check if the student has already purchased the course
                var existingEnrollment = _context.StudentCourses.FirstOrDefault(sc => sc.CourseId == courseId && sc.StudentId == studentId);
                if (existingEnrollment != null)
                {
                    return BadRequest("You have already purchased this course.");
                }

                // Create a new enrollment
                var studentCourse = new StudentCourse
                {
                    CourseId = courseId,
                    StudentId = (int)studentId,
                    PurchaseDate = DateTime.Now
                };

                _context.StudentCourses.Add(studentCourse);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error (consider using a logging library)
                Console.WriteLine(ex.Message); // Log to console for development purposes
                return StatusCode(500, "Internal server error. Please try again later."); // Return a user-friendly message
            }
        }


        public IActionResult CompleteCourse(int studentCourseId)
        {
            try
            {
                var studentCourse = _context.StudentCourses.Find(studentCourseId);
                if (studentCourse == null)
                {
                    return NotFound("Course not found.");
                }

                studentCourse.IsCompleted = true;
                _context.SaveChanges();

                // Automatically download certificate
                return RedirectToAction("Certificate", new { studentCourseId });
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        public IActionResult Certificate(int studentCourseId)
        {
            var studentCourse = _context.StudentCourses.Include(sc => sc.Course)
                .FirstOrDefault(sc => sc.Id == studentCourseId);

            if (studentCourse == null || !studentCourse.IsCompleted)
            {
                return NotFound(); // Return an error if the course is not completed
            }

            ViewBag.CurrentDateTime = DateTime.Now; // Pass the current date and time to the view
            return View(studentCourse); // Return a view to generate the certificate
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToAction("Login", "User"); // Redirect to the login page
        }

    }
}
