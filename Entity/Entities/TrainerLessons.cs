using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
     public class TrainerLessons
    {
        [Key]
        public int Id { get; set; }
        public bool Status { get; set; }        
        public int TrainerId { get; set; }
        public virtual AppUser Trainer { get; set; }
        public int EducationId { get; set; }
        public virtual Education Education { get; set; }
    }
}
