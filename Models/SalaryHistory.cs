using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web1.Models
{
    public class SalaryHistory
    {
        [Key]
        public int SalaryHistoryId { get; set; } // Primary key

        [Required]
        public string EmployeeName { get; set; } = string.Empty; // Employee name

        [Required]
        public int EmployeeId { get; set; } // Employee ID

        // Mark Department as nullable if it might not always have a value
        public string? Department { get; set; } // Department name

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalaryAmount { get; set; } // Salary amount

        [Required]
        public DateTime EffectiveDate { get; set; } // Effective date of the salary
    }
}
