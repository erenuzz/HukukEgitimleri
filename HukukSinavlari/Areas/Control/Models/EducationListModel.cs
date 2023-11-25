namespace HukukSinavlari.Areas.Control.Models
{
    public class EducationListModel
    {
        public int Id { get; set; }
        public string EducationName { get; set; }
        public DateTime StartDate { get; set; }      
        public string Description { get; set; }
        public decimal Price { get; set; }      
        public string Image { get; set; }
        public string Link { get; set; }
        public bool Status { get; set; }
    }
}
