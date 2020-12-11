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
    public class PeopleController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public PeopleController(ContosoUniversityContext context)
        {
            _context = context;
        }

        #region == 基本操作 ==
        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.People.ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, UpdatePersonViewModel person)
        {
            Person model = await _context.People.SingleOrDefaultAsync(x => x.Id == id);
            if (model == null) return BadRequest();

            model.InjectFrom(person);

            if (model.Discriminator.ToLower() == "Instructor".ToLower())
            {
                var office = await _context.OfficeAssignments.SingleOrDefaultAsync(x => x.InstructorId == model.Id);
                if (office == null) _context.Add(new OfficeAssignment { InstructorId = model.Id, Location = person.Office });
                else office.Location = person.Office;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(CreatePersonViewModel person)
        {
            Person model = new Person();
            model.InjectFrom(person);
            _context.People.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = model.Id }, model);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
        #endregion
    }
}
