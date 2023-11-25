using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
    public class AppUser:IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }     
        public string Mail { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public string UnencryptedPass { get; set; }
        public bool State { get; set; }
        public bool PaymentStatus { get; set; }
        public int? EducationId { get; set; }
        public virtual Education Education { get; set; }
    }
}
