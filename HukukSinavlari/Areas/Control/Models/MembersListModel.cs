using Entity.Entities;

namespace HukukSinavlari.Areas.Control.Models
{
    public class MembersListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool State { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string[] Role { get; set; }
    }
}
