using System.ComponentModel.DataAnnotations; // Import required for [Key]
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // Optional for advanced EF configurations

namespace web1.Models
{
    public class Employee
    {
        [Key] // Marks this property as the primary key
        public int EmployeeId { get; set; }

        [Required] // Ensures the Name property is mandatory
        [MaxLength(100)] // Limits the length of Name to 100 characters
        public string Name { get; set; } = string.Empty;

        [Required] // Ensures the Email property is mandatory
        [EmailAddress] // Validates the format as a proper email address
        public string Email { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Department")] // Establishes a foreign key relationship
        public int DepartmentId { get; set; }

        [ValidateNever]
        public Department Department { get; set; } = null!; // Navigation property for the related Department
    }
}
