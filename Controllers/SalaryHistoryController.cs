using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web1.Data;
using web1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace web1.Controllers
{
    [Authorize]
    public class SalaryHistoryController : Controller
    {
        private readonly AppDbContext _context;

        public SalaryHistoryController(AppDbContext context)
        {
            _context = context;
        }

        // Populate SalaryHistory with sample data
        public async Task<IActionResult> Populate()
        {
            // Clear existing records
            var existingRecords = _context.SalaryHistories.ToList();
            _context.SalaryHistories.RemoveRange(existingRecords);
            await _context.SaveChangesAsync();

            // Generate 2000 salary history records
            var random = new Random();
            var salaryHistories = new List<SalaryHistory>();
            var employees = _context.Employees.Include(e => e.Department).ToList();

            // Ensure all employees have valid departments
            if (employees.Any(e => e.Department == null))
            {
                TempData["ErrorMessage"] = "Some employees do not have an associated department. Populate cannot proceed.";
                return RedirectToAction("Index");
            }

            for (int i = 1; i <= 2000; i++)
            {
                var employee = employees[random.Next(employees.Count)];

                // Ensure the department is not null
                if (employee.Department != null)
                {
                    salaryHistories.Add(new SalaryHistory
                    {
                        EmployeeName = employee.Name,
                        EmployeeId = employee.EmployeeId,
                        Department = employee.Department.Name, // Use the department name
                        SalaryAmount = random.Next(50000, 120000),
                        EffectiveDate = DateTime.Now.AddDays(-random.Next(1, 730))
                    });
                }
            }

            try
            {
                _context.SalaryHistories.AddRange(salaryHistories);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Successfully populated 2000 Salary History records!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error populating Salary History: {ex.Message}";
            }

            return RedirectToAction("Index");
        }


        // GET: View all salary history records
        public IActionResult Index(string? departmentName)
        {
            // Query salary histories
            var salaryHistories = _context.SalaryHistories.AsQueryable();

            if (!string.IsNullOrEmpty(departmentName))
            {
                salaryHistories = salaryHistories.Where(sh => sh.Department == departmentName);
            }

            // Pass available departments to the view for filtering
            ViewBag.Departments = _context.Departments.Select(d => d.Name).ToList();

            return View(salaryHistories.ToList());
        }

        // POST: Clear all salary history records
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            try
            {
                var salaryHistories = _context.SalaryHistories.ToList();
                _context.SalaryHistories.RemoveRange(salaryHistories);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "All salary history entries have been cleared successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error clearing salary history: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
