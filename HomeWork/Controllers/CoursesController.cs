using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeWork.Entity;
using HomeWork.ViewModels;
using Omu.ValueInjecter;

namespace HomeWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        #region === 基本操作 ===
        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, UpdateCourseViewModel course)
        {
            Course model = new Course
            {
                CourseId = id,
            };
            model.InjectFrom(course);
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CreateCourseViewModel course)
        {
            Course model = new Course();
            model.InjectFrom(course);
            _context.Courses.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = model.CourseId }, model);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
        #endregion

        #region === 關聯操作 ===
        [HttpPost("AddInstructors/{id}")]
        public async Task<IActionResult> AddInstructors(int id, AddCourseInstructorsViewModel instructors)
        {
            var models = instructors.InstructorID.Select(x => new CourseInstructor
            {
                CourseId = id,
                InstructorId = x,
            });
            await _context.CourseInstructors.AddRangeAsync(models);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("RemoveInstructors/{id}")]
        public async Task<IActionResult> RemoveInstructors(int id, RemoveCourseInstructorsViewModel instructors)
        {
            var models = instructors.InstructorID.Select(x => new CourseInstructor
            {
                CourseId = id,
                InstructorId = x,
            });
            _context.CourseInstructors.RemoveRange(models);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("UpdateGrade/{id}")]
        public async Task<IActionResult> UpdateGrade(int id, UpdateGradeViewModel model)
        {
            Course course = await _context.Courses.SingleOrDefaultAsync(x => x.CourseId == id);
            if (course == null) return BadRequest();
            Person person = await _context.People.SingleOrDefaultAsync(x => x.Id == model.StudentId);
            if (person == null) return BadRequest();

            if (person.Discriminator.ToLower() == "Student".ToLower())
            {
                var grade = await _context.Enrollments.SingleOrDefaultAsync(x => x.CourseId == id && x.StudentId == person.Id);
                if (grade == null) _context.Add(new Enrollment { CourseId = id, StudentId = person.Id, Grade = model.Grade });
                else grade.Grade = model.Grade;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region === 檢視表 ===
        [HttpGet("GetStudentCount")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetStudentCount()
        {
            return await _context.VwCourseStudentCounts.ToListAsync();
        }
        [HttpGet("GetStudents")]
        public async Task<ActionResult<IEnumerable<VwCourseStudent>>> GetStudents()
        {
            return await _context.VwCourseStudents.ToListAsync();
        }
        #endregion
    }

}