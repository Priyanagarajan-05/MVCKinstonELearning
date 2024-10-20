using System.Collections.Generic;

namespace coursemvc.Models
{
    public class StudentDashboardViewModel
    {
        public List<Course> AllCourses { get; set; }
        public List<StudentCourse> MyCourses { get; set; }
    }
}
