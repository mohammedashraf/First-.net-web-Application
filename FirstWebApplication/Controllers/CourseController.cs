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
    public class CourseController : ControllerBase
    {
    
        private readonly NewDbContext dbContext;

        public CourseController(NewDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult AddCourse(CourseDTO  dto)
        {
            var Course = new  Course
            { 
                Title = dto.Title,
                Credits = dto.Credits,
                DepartmentID=dto.DepartmentID,
            };
            dbContext.Courses.Add(Course);
            dbContext.SaveChanges();
            return Ok(Course);
        }

     
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteCourse(long? id)
        {
            var todoItem = await dbContext.Courses.Include(x => x.Enrollements)
                .ThenInclude(x => x.Student)
                .FirstOrDefaultAsync(x => x.CourseID == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            dbContext.Courses.Remove(todoItem);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(long id, CourseDTO todoItemDTO)
        {
            //دي غالبا ملهاش لازمه ممكن تتشال
            if (id != todoItemDTO.CourseID)
            {
                return BadRequest();
            }
            var todoItem = await dbContext.Courses.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            todoItem.Title= todoItemDTO.Title;
            todoItem.Credits = todoItemDTO.Credits;
            todoItem.DepartmentID = todoItemDTO.DepartmentID;
            await dbContext.SaveChangesAsync();
            return NoContent();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetCourse(int? id)
        {
            var todoItem = await dbContext.Courses.Include(x => x.Enrollements)
                .ThenInclude(x => x.Student)
                .FirstOrDefaultAsync(x => x.CourseID == id);
            var todoItem2 = await dbContext.Courses.Include(x => x.CourseAssignments)
               .ThenInclude(x => x.Instructor)
               .FirstOrDefaultAsync(x => x.CourseID == id);

            if (todoItem == null)
            {
                return NotFound();
            }
            return Ok(new CourseResultall
            {
                ID = todoItem.CourseID,
                Name = todoItem.Title,
                Students = todoItem.Enrollements.Select(x=>
                new StudentResultall {ID=x.Student.ID,FirstName=x.Student.FirstMidName,LastName=x.Student.LastName}).ToList(),
                Instructors=todoItem2.CourseAssignments.Select(x=>
                new InstructorResultall { ID=x.Instructor.ID,FirstMidName=x.Instructor.FirstMidName,HireDate=x.Instructor.HireDate,LastName=x.Instructor.LastName}).ToList(),
                
            });
        }
        public class CourseResultall
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public List<InstructorResultall> Instructors { get; set; }
            public List<StudentResultall> Students { get; set; }
         
        }
        public class InstructorResultall
        {
            public int ID { get; set; }
            public string LastName { get; set; }
            public string FirstMidName { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime HireDate { get; set; }
        }
        public class StudentResultall
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

        }


    }
   }

