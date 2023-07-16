using RentNDrive.Models;

namespace RentNDrive.ImplementationLogics.IBusinessLogic
{
	public interface IVehicle
	{		
		VehicleInfo GetListById(int Id=0);
		List<VehicleInfo> GetList();
		BookInfo GetBookListById(int Id);
		List<BookInfo> GetBookList();
		Payment GetPaymentListById(int Id);
		List<Payment> GetPaymentList();
		Comment GetCommentListById(int Id);
		List<Comment> GetCommentList();
		List<Survey> GetSurveyList();
		Chat GetChatListById(int Id);
		List<Chat> GetChatList();		
	}
}
