using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeWork.Entity;
using HomeWork.ViewModels;

namespace HomeWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public DepartmentsController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, CreateDepartmentViewModel department)
        {
            var model = await _context.Departments.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXECUTE [dbo].[Department_Update] {id},{department.Name},{department.Budget},{department.StartDate},{department.InstructorId},{model.RowVersion}");

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(CreateDepartmentViewModel department)
        {
            var result = (await _context.CreateDepartmentResult.FromSqlInterpolated(
                $"EXECUTE [dbo].[Department_Insert] { department.Name},{department.Budget},{department.StartDate},{department.InstructorId}")
                .ToListAsync()).Single();
            var model = await _context.Departments.SingleAsync(x => x.DepartmentId == result.DepartmentId);

            return CreatedAtAction("GetDepartment", new { id = model.DepartmentId }, model);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE [dbo].[Department_Delete] {id},{department.RowVersion}");

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
