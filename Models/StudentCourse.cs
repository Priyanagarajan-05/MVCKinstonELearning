using System.ComponentModel.DataAnnotations;

namespace coursemvc.Models
{
    public class StudentCourse
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; } // Foreign key for Course
        public int StudentId { get; set; } // Foreign key for User

        public DateTime PurchaseDate { get; set; }
        public bool IsCompleted { get; set; } = false;

        public Course Course { get; set; } // Navigation property
        public User Student { get; set; } // Navigation property
    }
}
