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
    public class DepartmentController : ControllerBase
    {
        private readonly NewDbContext dbContext;

        public DepartmentController(NewDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult AddDepartment(DepartmentDTO dto)
        { 
            dbContext.Departments.Add(
                new Department
                {
                    Name = dto.Name,
                    Budget = dto.Budget,
                    StartDate = dto.StartDate,
                    InstructorID = dto.InstructorID,
                });
            dbContext.SaveChanges();
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int? id)
        {
            var todoItem = await dbContext.Departments.Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.DepartmentID == id);

            if (todoItem == null)
            {
                return NotFound();
            }
            return Ok(new Resultdep
            {
                ID = todoItem.DepartmentID,
                Name = todoItem.Name,
                Budget = todoItem.Budget,
                Courses = todoItem.Courses.Select(x =>
                new CourseResult { ID = x.CourseID, Name = x.Title })
                .ToList()
            });

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Department>> DeleteDepartment(int? id)
        {
            var todoItem = await dbContext.Departments.Include(x => x.Administrator)
                .ThenInclude(x => x.CourseAssignments)
                .FirstOrDefaultAsync(x => x.DepartmentID == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            dbContext.Departments.Remove(todoItem);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, DepartmentDTO todoItemDTO)
        {
            ////دي غالبا ملهاش لازمه ممكن تتشال
            if (todoItemDTO.Id.HasValue && id != todoItemDTO.Id)
            {
                return BadRequest();
            }
            var todoItem = await dbContext.Departments.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            todoItem.Name = todoItemDTO.Name;
            todoItem.Budget = todoItemDTO.Budget;
            todoItem.StartDate = todoItemDTO.StartDate;
            todoItem.InstructorID = todoItemDTO.InstructorID;
            dbContext.Departments.Update(todoItem);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        public class Resultdep
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public decimal Budget { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime StartDate { get; set; }

            public int? InstructorID { get; set; }
            public List<CourseResult> Courses { get; set; }
        }
    

    }
}
