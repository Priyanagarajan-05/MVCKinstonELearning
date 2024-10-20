/*
using coursemvc.Models;
using Microsoft.AspNetCore.Mvc;
using coursemvc.Data; // Ensure this points to the correct namespace for ApplicationDbContext


namespace coursemvc.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateCourse(Course course, string professorEmail)
        {
            // Get the ProfessorId based on email
            int? professorId = _context.GetProfessorIdByEmail(professorEmail);

            if (professorId == null)
            {
                // Handle the case where the professor is not found
                ModelState.AddModelError("ProfessorEmail", "Professor not found.");
                return View(course);
            }

            course.ProfessorId = (int)professorId; // Set the ProfessorId

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}*/


/* ---professor id not fetched ----
 * 
using coursemvc.Models;
using Microsoft.AspNetCore.Mvc;
using coursemvc.Data; // Ensure this points to the correct namespace for ApplicationDbContext
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace coursemvc.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Course/CreateCourse
        [HttpGet]
        public IActionResult CreateCourse()
        {
            // Get the logged-in user's email from the session
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User"); // If not logged in, redirect to login
            }

            // Find the professor (User) by email
            var professor = _context.Users.FirstOrDefault(u => u.Email == email);
            if (professor == null || professor.Role != "Professor")
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            // Create a new Course instance and set the ProfessorId (UserId from User table)
            var model = new Course
            {
                IsActive = false, // Default to false
                ProfessorId = professor.UserId // Automatically assign the ProfessorId
            };

            return View(model);
        }

        // POST: Course/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCourse(Course course, string professorEmail)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            // Get the ProfessorId based on email, professorEmail comes from form input
            var professor = _context.Users.FirstOrDefault(u => u.Email == professorEmail);
            if (professor == null)
            {
                // Handle the case where the professor is not found
                ModelState.AddModelError("ProfessorEmail", "Professor not found.");
                return View(course);
            }

            course.ProfessorId = professor.UserId; // Set the ProfessorId

            // Ensure that IsActive is set to false explicitly
            course.IsActive = false;

            // Add the course to the database
            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("Index", "Professor"); // Redirect to the professor's dashboard
        }
    }
}*/

/* -- course - -- fetch id ----
using Microsoft.AspNetCore.Mvc;
using coursemvc.Data;
using coursemvc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace coursemvc.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Course/CreateCourse
        [HttpGet]
        public IActionResult CreateCourse()
        {
            // Get the logged-in user's email from the session
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User"); // If not logged in, redirect to login
            }

            // Find the professor (User) by email
            var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");
            if (professor == null)
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            // Create a new Course instance and set the ProfessorId (UserId from User table)
            var model = new Course
            {
                IsActive = false, // Default to false
                ProfessorId = professor.UserId // Automatically assign the ProfessorId
            };

            return View(model);
        }

        // POST: Course/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                course.IsActive = false; // Set the IsActive property to false explicitly

                _context.Courses.Add(course);
                _context.SaveChanges(); // Save the course to the database

                return RedirectToAction("Index", "Professor"); // Redirect to the professor's dashboard
            }

            return View(course); // Return the same view with the invalid model
        }
    }
}*/



/* ------ not working 
 * 
using Microsoft.AspNetCore.Mvc;
using coursemvc.Data;
using coursemvc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace coursemvc.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Course/CreateCourse
        [HttpGet]
        public IActionResult CreateCourse()
        {
            // Get the logged-in user's email from the session
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User"); // If not logged in, redirect to login
            }

            // Find the professor (User) by email
            var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");
            if (professor == null)
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            // Create a new Course instance and set the ProfessorId (UserId from User table)
            var model = new Course
            {
                IsActive = false, // Default to false
                ProfessorId = professor.UserId // Automatically assign the ProfessorId
            };

            return View(model);
        }
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                // Get the logged-in user's email from the session
                var email = HttpContext.Session.GetString("UserEmail");
                var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");

                if (professor != null)
                {
                    // Set the ProfessorId from the retrieved professor
                    course.ProfessorId = professor.UserId;
                }

                // Set IsActive to false (0) when the course is created by the admin
                course.IsActive = false; // Explicitly set IsActive to false (0)

                try
                {
                    _context.Courses.Add(course);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Professor");
                }
                catch (Exception ex)
                {
                    // Log the error (you might use a logging framework here)
                    ModelState.AddModelError("", "Unable to save course. " + ex.Message);
                }
            }

            // Return the same view with the invalid model
            return View(course);
        }

    }
}

*/






/*========================  course form opening but not adding in database =========
 * 
using Microsoft.AspNetCore.Mvc;
using coursemvc.Data;
using coursemvc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace coursemvc.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Course/CreateCourse
        [HttpGet]
        public IActionResult CreateCourse()
        {
            // Get the logged-in user's email from the session
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User"); // If not logged in, redirect to login
            }

            // Find the professor (User) by email
            var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");
            if (professor == null)
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            // Create a new Course instance and set the ProfessorId (UserId from User table)
            var model = new Course
            {
                IsAccount = false, // Default to false
                ProfessorId = professor.UserId // Automatically assign the ProfessorId
            };

            return View(model);
        }

        // POST: Course/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCourse(Course course)
        {
            // Set IsAccount to false when the course is created
            course.IsAccount = false; // Default to false

            if (ModelState.IsValid)
            {
                // Get the logged-in user's email from the session
                var email = HttpContext.Session.GetString("UserEmail");
                var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");

                if (professor != null)
                {
                    // Set the ProfessorId from the retrieved professor
                    course.ProfessorId = professor.UserId;
                }

                try
                {
                    _context.Courses.Add(course);
                    _context.SaveChanges();
                    ViewBag.StatusMessage = "Course created successfully!";
                    return RedirectToAction("Index", "Professor");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save course. " + ex.Message);
                    ViewBag.StatusMessage = "Error: " + ex.Message; // Set the error message to display
                }
            }
            else
            {
                ViewBag.StatusMessage = "Please correct the errors and try again."; // Set error message if ModelState is invalid
            }

            // Return the same view with the invalid model
            return View(course);
        }


        // GET: Course/ManageCourses
        [HttpGet]
        public IActionResult ManageCourses()
        {
            // Retrieve only courses with IsAccount set to false
            var courses = _context.Courses.Where(c => !c.IsAccount).ToList();

            // Return the view with the list of courses
            return View(courses);
        }


    }
}
*/


using Microsoft.AspNetCore.Mvc;
using coursemvc.Data;
using coursemvc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace coursemvc.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Course/CreateCourse
        [HttpGet]
        public IActionResult CreateCourse()
        {
            // Get the logged-in user's email from the session
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User"); // If not logged in, redirect to login
            }

            // Find the professor (User) by email
            var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");
            if (professor == null)
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            // Create a new Course instance
            var model = new Course
            {
                IsAccount = false, // Default to false
                ProfessorId = professor.UserId // Automatically assign the ProfessorId
            };

            return View(model);
        }

        // POST: Course/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCourse(Course course)
        {
            course.IsAccount = false; // Explicitly set IsAccount to false

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Courses.Add(course);
                    _context.SaveChanges();
                    ViewBag.StatusMessage = "Course created successfully!";
                    return RedirectToAction("Index", "Professor");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save course. " + ex.Message);
                    ViewBag.StatusMessage = "Error: " + ex.Message; // Error message passed to the view
                }
            }

            // If model validation fails or exception occurs, return the view with the model and status message
            return View(course);
        }


        // GET: Course/MyCourses
        /*
        [HttpGet]
        public IActionResult MyCourses()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var professor = _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");
            if (professor == null)
            {
                return RedirectToAction("Login", "User"); // If not a professor, redirect to login
            }

            var courses = _context.Courses.Where(c => c.ProfessorId == professor.UserId).ToList();
            return View(courses); // Return the list of courses for the logged-in professor
        }*/


        // GET: Course/ManageCourses
        [HttpGet]
        public IActionResult ManageCourses()
        {
            var courses = _context.Courses.Where(c => !c.IsAccount).ToList(); // Get courses with IsAccount = false
            return View(courses);
        }

        // POST: Course/AcceptCourse
        [HttpPost]
        public IActionResult AcceptCourse([FromBody] CourseRequest courseRequest)
        {
            if (courseRequest == null || courseRequest.CourseId <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            // Logic to accept the course
            var course = _context.Courses.Find(courseRequest.CourseId);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            course.IsAccount = true; // Assuming this is how you accept a course
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult RejectCourse([FromBody] CourseRequest courseRequest)
        {
            if (courseRequest == null || courseRequest.CourseId <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            // Logic to reject the course
            var course = _context.Courses.Find(courseRequest.CourseId);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            course.IsAccount = false; // Assuming this is how you reject a course
            _context.SaveChanges();

            return Ok();
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


    }
}
