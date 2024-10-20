/*
using Microsoft.AspNetCore.Mvc;
using coursemvc.Data;
using coursemvc.Models; // Ensure you're using the correct namespace
using Microsoft.AspNetCore.Http; // For session management
using System.Linq;

namespace coursemvc.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /User/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Ensure IsActive is set to false by default
                user.IsActive = false;

                _context.Users.Add(user);
                _context.SaveChanges();  // Save user to the database

                // Redirect to Login page after successful registration
                return RedirectToAction("Login", "User");
            }

            return View(user);
        }

        // GET: /User/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and Password are required.";
                return View();
            }

            // Retrieve the user from the database based on email and password
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid login attempt.";
                return View();
            }

            // Set the user's name in TempData for the dashboard
            TempData["UserName"] = user.Name;

            // Set the user's email in the session
            HttpContext.Session.SetString("UserEmail", user.Email);

            // Redirect based on the role
            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin");

                case "Professor":
                    return RedirectToAction("Index", "Professor");

                case "Student":
                    return RedirectToAction("Index", "Student");

                default:
                    ViewBag.Error = "Invalid role.";
                    return View();
            }
        }

        // POST: /User/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}
*/

using Microsoft.AspNetCore.Mvc;
using coursemvc.Data;
using coursemvc.Models; // Ensure you're using the correct namespace
using Microsoft.AspNetCore.Http; // For session management
using System.Linq;

namespace coursemvc.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /User/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Ensure IsActive is set to false by default
                user.IsActive = false;

                _context.Users.Add(user);
                _context.SaveChanges();  // Save user to the database

                // Redirect to Login page after successful registration
                return RedirectToAction("Login", "User");
            }

            return View(user);
        }

        // GET: /User/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and Password are required.";
                return View();
            }

            // Retrieve the user from the database based on email and password
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid login attempt.";
                return View();
            }

            // Set user details in session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserId", user.UserId.ToString()); // Store UserId in session
            TempData["UserName"] = user.Name; // Optional: Store User Name in TempData

            // Redirect based on the role
            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                case "Professor":
                    return RedirectToAction("Index", "Professor");
                case "Student":
                    return RedirectToAction("Index", "Student");
                default:
                    ViewBag.Error = "Invalid role.";
                    return View();
            }
        }


        // POST: /User/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}
