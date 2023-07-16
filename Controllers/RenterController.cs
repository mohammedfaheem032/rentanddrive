using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentNDrive.Data;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;
using RentNDrive.Models.UIModel;
using System.Security.Claims;

namespace RentNDrive.Controllers
{
    public class RenterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserInfo> _userManager;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IVehType _vehtypeR;
        private readonly IVehManufacturer _vehmanR;
        private readonly IUser _userR;
        private readonly IVehicle _vehicleR;
        private readonly IWebHostEnvironment _env;
        public RenterController(UserManager<UserInfo> userManager, SignInManager<UserInfo> signInManager,
                RoleManager<IdentityRole> roleManager, IUser userR, ApplicationDbContext context, IVehType vehtypeR, IVehManufacturer vehmanR, IVehicle vehicleR, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _vehtypeR = vehtypeR;
            _vehmanR = vehmanR;
            _userR = userR;
            _vehicleR = vehicleR;
            _env = env;            
        }
        public IActionResult BookNow(VehicleView vehicleViewModel, int vehId)
        {
            var CurrentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            vehicleViewModel.GetAllBookingHistory = User.IsInRole("Renter") ? _vehicleR.GetBookList().Where(x => x.RenterId == CurrentUserId).ToList() :
                User.IsInRole("Admin") ? _vehicleR.GetBookList().ToList() : _vehicleR.GetBookList().ToList();
            if (CurrentUserId != null && User?.Identity?.IsAuthenticated == true)
            {            
                vehicleViewModel.GetVehicleInfo = _vehicleR.GetListById(vehId);
                vehicleViewModel.GetuserInfo = _userR.GetUserById(CurrentUserId);
                vehicleViewModel.GetAllBookingHistory.ForEach(v =>
                {
                    var ownerId = _vehicleR.GetListById(v.VehicleId).OwnerId;
                    v.NMOwnerName = _userR.GetUserById(ownerId).OwnerName;
                    v.NMVehicleName = _vehicleR.GetListById(v.VehicleId).VehicleName;
                    v.NMRenterName = _userR.GetUserById(v.RenterId).FirstName;
                });
                return View("BookInfo", vehicleViewModel);
            }
            return RedirectToAction("Login", "Authentication");
        }

        [HttpPost]
        public async Task<IActionResult> BookNow(VehicleView vehicleView)
        {
            if (vehicleView.GetBookInfo.DropOffDate == null)
            {
                return View("BookInfo", vehicleView);
            }
            if (vehicleView.GetVehicleInfo.Id >0)
            {                
                vehicleView.GetBookInfo.RenterId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                vehicleView.GetBookInfo.VehicleId = vehicleView.GetVehicleInfo.Id;
                vehicleView.GetBookInfo.PaymentMethod = "";
                vehicleView.GetBookInfo.PaymentStatus = "Not Paid";
                var priceType = _vehicleR.GetListById(vehicleView.GetVehicleInfo.Id).PriceType; 
                var priceAmount = _vehicleR.GetListById(vehicleView.GetVehicleInfo.Id).VehiclePrice;
                switch (priceType)
                {
                    case "Monthly":
                        var durationMonth = (vehicleView.GetBookInfo.DropOffDate.Date - vehicleView.GetBookInfo.PickUpDate.Date).TotalDays;
                        priceAmount = priceAmount / 30;
                        priceAmount = (decimal)durationMonth * priceAmount;
                        break;
                    case "Daily":
                        var durationDay = (vehicleView.GetBookInfo.DropOffDate.Date - vehicleView.GetBookInfo.PickUpDate.Date).TotalDays;
                        priceAmount = (decimal)durationDay * priceAmount;
                        break;
                    default://Hour
                        var durationHour = (vehicleView.GetBookInfo.DropOffDate - vehicleView.GetBookInfo.PickUpDate).TotalHours;
                        priceAmount = (decimal)durationHour * priceAmount;
                        break;
                }
                vehicleView.GetBookInfo.TotalAmount = priceAmount;
                var idval = vehicleView.GetBookInfo.Id > 0 ? _context.Update(vehicleView.GetBookInfo) : _context.Add(vehicleView.GetBookInfo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("BookNow", vehicleView);
        }
        public IActionResult Payment(int bookId=0)
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            VehicleView vehicleView = new VehicleView();
            vehicleView.GetBookInfo = _vehicleR.GetBookListById(bookId);
            vehicleView.GetAllPaymentHistory = User.IsInRole("Renter") ? _vehicleR.GetPaymentList().Where(x => x.RenterId == user).ToList() :
                User.IsInRole("Admin") ? _vehicleR.GetPaymentList().ToList() : _vehicleR.GetPaymentList().ToList();
            
            //vehicleView.GetAllBookingHistory = _vehicleR.GetBookList().Where(x => x.RenterId == user).ToList();
            //vehicleView.GetAllPaymentHistory = _vehicleR.GetPaymentList();
            if (bookId>0)
                vehicleView.GetVehicleInfo = _vehicleR.GetListById(_vehicleR.GetBookListById(bookId).VehicleId);

            if (vehicleView.GetAllPaymentHistory.Count > 0 && user != null && User?.Identity?.IsAuthenticated == true)
            {
                //vehicleView.GetVehicleInfo = _vehicleR.GetListById((_vehicleR.GetBookListById((int)bookId).VehicleId));
                vehicleView.GetuserInfo = _userR.GetUserById(user);
                vehicleView.GetAllPaymentHistory.ForEach(v =>
                {
                    var bookinginfo = _vehicleR.GetBookListById(v.BookingId);
                    var vehId = _vehicleR.GetBookListById(v.BookingId).VehicleId;
                    var ownerId = _vehicleR.GetListById(vehId).OwnerId;
                    v.NMOwnerName = _userR.GetUserById(ownerId).OwnerName;
                    v.NMVehicleTitle = _vehicleR.GetListById(vehId).VehicleName;
                    v.NMPickUpDate = bookinginfo.PickUpDate;
                    v.NMDropOffDate = bookinginfo.PickUpDate;
                    v.NMDropLocation = bookinginfo.DropOffLocation;
                    v.NMPickUpLocation = bookinginfo.PickUpLocation;
                    v.NMPaymentPrice = bookinginfo.TotalAmount;
                    v.NMRenterName = _userR.GetUserById(v.RenterId).FirstName;
                });           
            }
            return View(vehicleView);
        }
        [HttpPost]
        public IActionResult Payment(VehicleView vehicleView)
        {
            if (vehicleView.GetPaymentInfo.CardName == null)
            {
                return View("Payment", vehicleView);
            }
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            vehicleView.GetPaymentInfo.PaymentDate = DateTime.Now;
            vehicleView.GetPaymentInfo.BookingId = vehicleView.GetBookInfo.Id;
            vehicleView.GetPaymentInfo.RenterId = user;
            _context.Add(vehicleView.GetPaymentInfo);  
            _context.SaveChanges();
            var paymentstatus = _vehicleR.GetBookListById(vehicleView.GetBookInfo.Id);
            if (paymentstatus != null)
            {
                paymentstatus.PaymentStatus = "Paid";
                paymentstatus.PaidDate = DateTime.Now;
                _context.Entry(paymentstatus).State = EntityState.Modified;                
                _context.SaveChanges();                
            }            
            _context.SaveChangesAsync();
            return RedirectToAction("Payment",new { bookId = vehicleView.GetBookInfo.Id });
        }
        public IActionResult PaymentHistory(VehicleView vehicleView)
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            vehicleView.GetAllPaymentHistory = _vehicleR.GetPaymentList().Where(u => u.RenterId == user).ToList();                 
            return View(vehicleView);
        }
        [HttpPost]
        public IActionResult Comment(VehicleView vehicleView)
        {            
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            vehicleView.GetCommentInfo.RenterId = user;
            vehicleView.GetCommentInfo.VehicleId = vehicleView.GetVehicleInfo.Id;
            vehicleView.GetCommentInfo.OwnerId = _vehicleR.GetListById(vehicleView.GetCommentInfo.VehicleId).OwnerId;
            vehicleView.GetCommentInfo.CommentsDate = DateTime.Now;
            _context.Add(vehicleView.GetCommentInfo);
            _context.SaveChanges();
            return RedirectToAction("VehicleDetails", "Home", new {id = vehicleView.GetVehicleInfo.Id });
        }
        [HttpPost]
        public IActionResult Survey(VehicleView vehicleView)
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            vehicleView.GetSurveyInfo.RenterId = user;
            vehicleView.GetSurveyInfo.VehicleId = vehicleView.GetVehicleInfo.Id;
            vehicleView.GetSurveyInfo.OwnerId = _vehicleR.GetListById(vehicleView.GetSurveyInfo.VehicleId).OwnerId;
            vehicleView.GetSurveyInfo.SurveyDate = DateTime.Now;
            _context.Add(vehicleView.GetSurveyInfo);
            _context.SaveChanges();
            return RedirectToAction("VehicleDetails", "Home", new { id = vehicleView.GetVehicleInfo.Id });
        }
    }
}
