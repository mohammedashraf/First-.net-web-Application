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

namespace FirstWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly NewDbContext dbContext;

        public StudentController(NewDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult AddStudent(StudentDTO dto)
        {
            dbContext.Students.Add(
                new Student
                { 
                    FirstMidName = dto.FirstMidName,
                    LastName = dto.LastName,
                    EnrollmentDate=dto.EnrollmentDate,
                });
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int? id)
        {
            var todoItem = await dbContext.Students.Include(x => x.Enrollements)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.ID == id);

            if (todoItem == null)
            {
                return NotFound();
            }
            return Ok(new Result
            {
                ID = todoItem.ID,
                FirstName=todoItem.FirstMidName,
                LastName = todoItem.LastName,
                Courses = todoItem.Enrollements.Select(x =>
                new CourseResult { ID = x.Course.CourseID, Name = x.Course.Title })
                .ToList()
            });
        }

   

        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(long? id)
        {
            var todoItem = await dbContext.Students.Include(x => x.Enrollements)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            dbContext.Students.Remove(todoItem);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentDTO todoItemDTO)
        {
            //دي غالبا ملهاش لازمه ممكن تتشال
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }
            var todoItem = await dbContext.Students.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            todoItem.FirstMidName = todoItemDTO.FirstMidName;
            todoItem.LastName = todoItemDTO.LastName;
            todoItem.EnrollmentDate = todoItemDTO.EnrollmentDate;
            dbContext.Students.Update(todoItem);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("Enroll-Course")]
        public IActionResult AddCourseToStudent(StudentCoursesDTO dto)
        {

            var student = new Enrollement
            {
                StudentID = dto.StudentId,
                CourseID = dto.CoursesIds,
                Grade = dto.Grade,
            };
            //Enum.GetValues(typeof(DayofWeek));
            dbContext.Enrollements.Add(student);
            dbContext.SaveChanges();
            return Ok(student);
        }
    }

    public class Result
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<CourseResult> Courses { get; set; }
    }

    public class CourseResult
    {

        public int ID { get; set; }
        public string Name { get; set; }
    }

}

