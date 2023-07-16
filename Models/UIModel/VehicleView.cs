namespace RentNDrive.Models.UIModel
{
	public class VehicleView : AdminView
	{
		public VehicleInfo GetVehicleInfo { get; set; }
		public List<VehicleInfo> GetAllVehicles { get; set; } = new List<VehicleInfo>();
		public BookInfo GetBookInfo { get; set; }
		public List<BookInfo> GetAllBookingHistory { get; set; }
		public Payment GetPaymentInfo { get; set; }
		public List<Payment> GetAllPaymentHistory { get; set; }
		public Comment GetCommentInfo { get; set; }
		public List<Comment> GetAllComments { get; set; }
		public Survey GetSurveyInfo { get; set; }
		public List<Survey> GetAllSurvey { get; set; }
		public bool IsPaid { get; set; }
		public Chat GetChatInfo { get; set; }
		public List<Chat> GetAllChat { get; set; }
	}
}
