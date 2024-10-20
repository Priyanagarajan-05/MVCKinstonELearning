using Microsoft.EntityFrameworkCore;
using coursemvc.Models;

namespace coursemvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }


        // Method to get ProfessorId by email
        public int? GetProfessorIdByEmail(string email)
        {
            var professor = Users.FirstOrDefault(u => u.Email == email && u.Role == "Professor");
            return professor?.UserId; // Return the UserId if found; otherwise, null
        }
    }
}
