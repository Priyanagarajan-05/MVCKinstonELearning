using System.ComponentModel.DataAnnotations;

namespace coursemvc.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; } // Primary key for the Course

        [Required]
        public string CourseName { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public bool IsAccount { get; set; } = false; // Default to true (1)

        [Required]
        public int ProfessorId { get; set; } // Foreign key for the Professor (Email as string)

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int Price { get; set; }  
    }
}
