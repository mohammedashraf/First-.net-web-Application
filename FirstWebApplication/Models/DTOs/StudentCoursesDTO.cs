using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApplication.Models.DTOs
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class StudentCoursesDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentCoursesDTOID { get; set; }
        public int StudentId { get; set; }
        public int CoursesIds { get; set; }
        public Grade? Grade { get; set; }
    }
}
