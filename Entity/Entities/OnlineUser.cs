using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
    public class OnlineUser
    {
        [Key]
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }
        public DateTime VisitDate { get; set; }

    }
}
