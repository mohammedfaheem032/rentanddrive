using RentNDrive.Data;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;

namespace RentNDrive.ImplementationLogics.BusinessLogic
{
    public class VehicleRep : IVehicle
    {
        private readonly ApplicationDbContext _con;
        public VehicleRep(ApplicationDbContext con)
        {
            _con = con;
        }

        public List<BookInfo> GetBookList()
        {
            return _con.BookInfo.ToList();
        }

        public BookInfo GetBookListById(int Id)
        {
            return _con.BookInfo.Where(e => e.Id == Id).FirstOrDefault();

        }

        public List<Comment> GetCommentList()
        {
            return _con.CommentsInfo.ToList();
        }
        public List<Survey> GetSurveyList() { return _con.SurveyInfo.ToList(); }


        public Comment GetCommentListById(int Id)
        {
            return _con.CommentsInfo.Where(e => e.VehicleId == Id).FirstOrDefault();
        }

        public List<VehicleInfo> GetList()
        {
            return _con.VehicleInfo.ToList();
        }

        public VehicleInfo GetListById(int Id = 0)
        {
            return _con.VehicleInfo.Where(e => e.Id == Id).FirstOrDefault();
        }

        public List<Payment> GetPaymentList()
        {
            return _con.PaymentInfo.ToList();
        }

        public Payment GetPaymentListById(int Id)
        {
            return _con.PaymentInfo.Where(e => e.Id == Id).FirstOrDefault();
        }

        public Chat GetChatListById(int Id)
        {
            return _con.ChatInfo.Where(e => e.VehicleId == Id).FirstOrDefault();
        }

        public List<Chat> GetChatList()
        {
            return _con.ChatInfo.ToList();
        }

    }
}
