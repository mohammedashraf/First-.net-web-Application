using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApplication.Models.DTOs
{
    public class CourseAssignmentDTO
    {
        public int CourseAssignmentDTOID { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
    }
}
