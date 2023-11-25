using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public string EducationName { get; set; }
        public DateTime StartDate { get; set; }      
        public decimal Price { get; set; }
        public string Description { get; set; }      
        public string Image { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }

    }
}
