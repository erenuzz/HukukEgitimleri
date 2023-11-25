using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
	public class PurchasedTrainings
	{
		[Key]
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int EducationId { get; set; }
        public virtual Education Education { get; set; }
        public bool PaymentStatus { get; set; }
        public bool EducationStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public bool CanceledTrainings { get; set; }
        public string PaymentType { get; set; }
    }
}
