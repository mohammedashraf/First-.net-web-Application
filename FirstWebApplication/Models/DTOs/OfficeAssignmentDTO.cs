using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApplication.Models.DTOs
{
    public class OfficeAssignmentDTO
    {
        [Key]
        public int InstructorID { get; set; }
        public string Location { get; set; }
    }
}
