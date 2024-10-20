using System.ComponentModel.DataAnnotations;

namespace coursemvc.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } // Auto-incremented in SQL

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } // This is the principal key for the relationship

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Professor, Student

        public bool IsActive { get; set; } = false; // Default to true (1)
    }
}
