using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FirstWebApplication.Models
{
    public partial class Course
    {
        public Course()
        {
           // Enrollements = new HashSet<Enrollement>();
        }
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public  ICollection<CourseAssignment> CourseAssignments { get; set; }
        public  ICollection<Enrollement> Enrollements { get; set; }
    }
}
