namespace HukukSinavlari.Areas.Control.Models
{
	public class RemittanceIncomingPaymentsModel
	{
		//Havale Gelen Ödemeler Modeli
		public int Id { get; set; }
		public string EducationName { get; set; }
		public DateTime StartDate { get; set; }
		public decimal Price { get; set; }
		public bool PaymentStatus { get; set; }
		public bool EducationStatus { get; set; }		
		public bool CanceledTrainings { get; set; }
        public string PaymentType { get; set; }
    }
}
