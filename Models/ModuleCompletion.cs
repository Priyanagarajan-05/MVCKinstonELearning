using System.ComponentModel.DataAnnotations;

namespace coursemvc.Models
{
    public class ModuleCompletion
    {
        [Key]
        public int CompletionId { get; set; } // Primary key for Module Completion

        [Required]
        public int ModuleId { get; set; } // Foreign key for Module

        [Required]
        public int StudentId { get; set; } // Foreign key for Student (UserId)

        [Required]
        public DateTime CompletionDate { get; set; } // Date of module completion

    }
}
