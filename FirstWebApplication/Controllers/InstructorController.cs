using FirstWebApplication.Models;
using FirstWebApplication.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstWebApplication.context;
using System.ComponentModel.DataAnnotations;

namespace FirstWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly NewDbContext dbContext;

        public InstructorController(NewDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult AddInstructor(InstructorDTO dto)
        {
            dbContext.Instructors.Add(
                new Instructor
                {
                    FirstMidName = dto.FirstMidName,
                    LastName = dto.LastName,
                    HireDate = dto.HireDate,
                });
            dbContext.SaveChanges();
            return Ok();
        }
     
        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructor(int? id)
        {
            var todoItem = await dbContext.Instructors.Include(x => x.CourseAssignments)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return Ok(new ResultInst
            {
                ID = todoItem.ID,
                FirstName = todoItem.FirstMidName,
                LastName = todoItem.LastName,
                HireDate = todoItem.HireDate,
                Courses = todoItem.CourseAssignments.Select(x =>
                new CourseResult { ID = x.Course.CourseID, Name = x.Course.Title })
                .ToList()
            });

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Instructor>> DeleteInstructor(int? id)
        {
            var todoItem = await dbContext.Instructors.Include(x => x.CourseAssignments)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            dbContext.Instructors.Remove(todoItem);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructor(int id, InstructorDTO todoItemDTO)
        {
            //دي غالبا ملهاش لازمه ممكن تتشال
            if (id != todoItemDTO.ID)
            {
                return BadRequest();
            }
            var todoItem = await dbContext.Instructors.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            todoItem.FirstMidName = todoItemDTO.FirstMidName;
            todoItem.LastName = todoItemDTO.LastName;
            todoItem.HireDate = todoItemDTO.HireDate;
            dbContext.Instructors.Update(todoItem);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("Enroll-Course")]
        public IActionResult AddCourseToInstructor(CourseAssignmentDTO dto)
        {
            var instructor = new CourseAssignment
            {
                InstructorID = dto.InstructorID,
                CourseID = dto.CourseID,
            };
            dbContext.CourseAssignments.Add(instructor);
            dbContext.SaveChanges();
            return Ok(instructor);
        }
        [HttpPut("Enroll-Office")]
        public IActionResult AddOfficeToInstructor(OfficeAssignmentDTO dto)
        {
            var instructor = new OfficeAssignment
            {
                InstructorID = dto.InstructorID,
                 Location = dto.Location,
            };
            dbContext.OfficeAssignments.Add(instructor);
            dbContext.SaveChanges();
            return Ok(instructor);
        }
        public class ResultInst
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime HireDate { get; set; }
            public List<CourseResult> Courses { get; set; }
        }

    }
}
