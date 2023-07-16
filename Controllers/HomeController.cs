using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentNDrive.Data;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;
using RentNDrive.Models.UIModel;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using System.Runtime.Serialization.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RentNDrive.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IUser _userRepo;
        private readonly IVehicle _vehicleRepo;
        private readonly IVehManufacturer _vehmanRepo;
        private readonly IVehType _vehtypeRepo;

        public HomeController(UserManager<UserInfo> userManager,
            SignInManager<UserInfo> signInManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context, IUser userRepos, IVehicle vehicleRepos, IVehManufacturer vehmanRepos, IVehType vehtypeRepos)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _userRepo = userRepos;
            _vehicleRepo = vehicleRepos;
            _vehmanRepo = vehmanRepos;
            _vehtypeRepo = vehtypeRepos;

        }
        public IActionResult Index(VehicleView vehicleViewModel)
        {
            vehicleViewModel.GetAllVehicles = _vehicleRepo.GetList().Where(x => x.FeatureVehicle == true && x.VehicleActiveStatus == true).ToList();
            return View(vehicleViewModel);
        }
        public IActionResult VehicleList(VehicleView vehicleViewModel)
        {
            vehicleViewModel.GetAllVehicles = _vehicleRepo.GetList().Where(x => x.VehicleActiveStatus == true).ToList();

            vehicleViewModel.GetAllVehicles.ForEach(v =>
            {
                v.NMVehicleManufacturer = _vehmanRepo.GetListById(v.VehicleManufacturer).VehicleManufacturerName;
                v.NMVehicleType = _vehtypeRepo.GetListById(v.VehicleType).VehicleTypeName;
                v.NMOwnerName = _userRepo.GetUserById(v.OwnerId).OwnerName;
            });
            return View(vehicleViewModel);
        }
        public IActionResult VehicleDetails(VehicleView vehicleViewModel, int? id)
        {
            vehicleViewModel.GetVehicleInfo = _vehicleRepo.GetListById((int)id);
            vehicleViewModel.GetAllVehicles = _vehicleRepo.GetList();
            vehicleViewModel.GetAllVehicles.ForEach(v =>
            {
                v.NMVehicleManufacturer = _vehmanRepo.GetListById(v.VehicleManufacturer).VehicleManufacturerName;
                v.NMVehicleType = _vehtypeRepo.GetListById(v.VehicleType).VehicleTypeName;
                v.NMOwnerName = _userRepo.GetUserById(v.OwnerId).OwnerName;
            });
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user != null)
            {
                vehicleViewModel.GetuserInfo = _userRepo.GetUserById(user);
                vehicleViewModel.IsPaid = string.IsNullOrEmpty(vehicleViewModel.GetuserInfo.Id) ? false : _vehicleRepo.GetBookList().Any(x => x.RenterId == vehicleViewModel.GetuserInfo.Id && x.VehicleId == (int)id && x.PaymentStatus == "Paid");
            }
            vehicleViewModel.GetAllComments = _vehicleRepo.GetCommentList().Where(u => u.VehicleId == (int)id).ToList();
            vehicleViewModel.GetAllComments.ForEach(c =>
            {
                c.RenterName = _userRepo.GetUserById(c.RenterId).FirstName;
            });
            var surveyreport = vehicleViewModel.GetAllSurvey = _vehicleRepo.GetSurveyList().Where(u => u.VehicleId == (int)id).ToList();
            if (surveyreport.Count > 0)
            {
                ViewBag.SurveyReport = surveyreport.Count();
                TempData["Q1SurveyCount"] = surveyreport.Where(u => u.Q1Survey == SurveyResult.Excellent).Count() +
                    " Excellent " + surveyreport.Where(u => u.Q1Survey == SurveyResult.Satisfactory).Count() +
                    " Satisfactory " + surveyreport.Where(u => u.Q1Survey == SurveyResult.Neutral).Count() +
                    " Neutral " + surveyreport.Where(u => u.Q1Survey == SurveyResult.NotGoodEnough).Count() +
                    " Not Good Enough " + surveyreport.Where(u => u.Q1Survey == SurveyResult.ExtremelyUnsatisfactory).Count() +
                    "Extremely Unsatisfactory";
                TempData["Q2SurveyCount"] = surveyreport.Where(u => u.Q2Survey == SurveyResult.Excellent).Count() +
                    " Excellent " + surveyreport.Where(u => u.Q2Survey == SurveyResult.Satisfactory).Count() +
                    " Satisfactory " + surveyreport.Where(u => u.Q2Survey == SurveyResult.Neutral).Count() +
                    " Neutral " + surveyreport.Where(u => u.Q2Survey == SurveyResult.NotGoodEnough).Count() +
                    " Not Good Enough " + surveyreport.Where(u => u.Q2Survey == SurveyResult.ExtremelyUnsatisfactory).Count() +
                    "Extremely Unsatisfactory";
                TempData["Q3SurveyCount"] = surveyreport.Where(u => u.Q3Survey == SurveyResult.Excellent).Count() +
                    " Excellent " + surveyreport.Where(u => u.Q3Survey == SurveyResult.Satisfactory).Count() +
                    " Satisfactory " + surveyreport.Where(u => u.Q3Survey == SurveyResult.Neutral).Count() +
                    " Neutral " + surveyreport.Where(u => u.Q3Survey == SurveyResult.NotGoodEnough).Count() +
                    " Not Good Enough " + surveyreport.Where(u => u.Q3Survey == SurveyResult.ExtremelyUnsatisfactory).Count() +
                    "Extremely Unsatisfactory";
                TempData["Q4SurveyCount"] = surveyreport.Where(u => u.Q4Survey == SurveyResult.Excellent).Count() +
                    " Excellent " + surveyreport.Where(u => u.Q4Survey == SurveyResult.Satisfactory).Count() +
                    " Satisfactory " + surveyreport.Where(u => u.Q4Survey == SurveyResult.Neutral).Count() +
                    " Neutral " + surveyreport.Where(u => u.Q4Survey == SurveyResult.NotGoodEnough).Count() +
                    " Not Good Enough " + surveyreport.Where(u => u.Q4Survey == SurveyResult.ExtremelyUnsatisfactory).Count() +
                    "Extremely Unsatisfactory";
            }
            //vehicleViewModel.GetCommentInfo = _vehicleRepo.GetCommentListById((int)id);           
            return View(vehicleViewModel);
        }
        // private DateTime pickdate = DateTime.Now;
        public IActionResult FindBookingVehicle(DateTime pickup, DateTime dropoff, string findlocation = "", string vehicleName = "", string vehicleType = "", string vehicleManufacturer = "")
        {
            VehicleView vehicleView = new VehicleView();
            vehicleView.GetAllVehicles = _vehicleRepo.GetList().Where(v => v.VehicleActiveStatus == true).ToList();
            var bookingList = _vehicleRepo.GetBookList();
            int bookingVehicleId = 0;
            vehicleView.GetAllBookingHistory = _vehicleRepo.GetBookList().Where(v => v.PaymentStatus == "Paid" && (v.PickUpDate >= pickup && v.DropOffDate <= dropoff)).ToList();
            for (int i = vehicleView.GetAllVehicles.Count - 1; i >= 0; i--)
            {
                if (vehicleView.GetAllBookingHistory.Any(x => x.VehicleId == vehicleView.GetAllVehicles[i].Id))
                {
                    vehicleView.GetAllVehicles.RemoveAt(i);
                }
            }
            if (!string.IsNullOrEmpty(findlocation))
                vehicleView.GetAllVehicles = vehicleView.GetAllVehicles.Where(x => x.Location.ToLower().Contains(findlocation.ToLower())).ToList();
            if (!string.IsNullOrEmpty(vehicleName))
                vehicleView.GetAllVehicles = vehicleView.GetAllVehicles.Where(x => x.VehicleName.ToLower().Contains(vehicleName.ToLower())).ToList();


            if (!string.IsNullOrEmpty(vehicleType))
            {
                var vehType = _vehtypeRepo.GetList().Where(x => x.VehicleTypeName.ToLower().Contains(vehicleType.ToLower())).ToList();
                var vehId = vehType.FirstOrDefault(x => x.VehicleTypeName.ToLower().Contains(vehicleType.ToLower())).Id;
                vehicleView.GetAllVehicles = vehicleView.GetAllVehicles.Where(x => x.VehicleType == vehId).ToList();
                //vehicleView.GetAllVehicles = vehicleList.Where(x => x.VehicleType.ToLower().Contains(vehicleType.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(vehicleManufacturer))
            {
                var vehManufacturer = _vehmanRepo.GetList().FirstOrDefault(x => x.VehicleManufacturerName.ToLower().Contains(vehicleManufacturer.ToLower())).Id;
                vehicleView.GetAllVehicles = vehicleView.GetAllVehicles.Where(x => x.VehicleManufacturer == vehManufacturer).ToList();
                //vehicleView.GetAllVehicles = vehicleList.Where(x => x.VehicleType.ToLower().Contains(vehicleType.ToLower())).ToList();
            }

            return View("VehicleList", vehicleView);

        }
        public IActionResult Chat(VehicleView vehicleView, int vehId = 0)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Authentication"); }
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            vehicleView.GetChatInfo = new Chat();

            if (vehId > 0)
            {
                vehicleView.GetChatInfo.VehicleNameNotMapped = _vehicleRepo.GetListById(vehId).VehicleName;
                vehicleView.GetChatInfo.OwnerNameNotMapped = _userRepo.GetUserById(_vehicleRepo.GetListById(vehId).OwnerId).OwnerName;
                vehicleView.GetChatInfo.VehicleId = vehId;
                vehicleView.GetChatInfo.OwnerId = _vehicleRepo.GetListById(vehId).OwnerId;
                vehicleView.GetChatInfo.RenterId = _vehicleRepo.GetChatListById(vehId)?.RenterId;
            }
            vehicleView.GetAllChat = _vehicleRepo.GetChatList().Where(x => x.OwnerId == user || x.RenterId == user).OrderByDescending(x=>x.SendDate).ToList();
            vehicleView.GetAllChat.ForEach(c =>
            {
                c.VehicleNameNotMapped = _vehicleRepo.GetListById((int)c.VehicleId).VehicleName;
                c.RenterNameNotMapped = _userRepo.GetUserById(c.RenterId).FirstName;
                c.OwnerNameNotMapped = _userRepo.GetUserById(c.OwnerId).OwnerName;
            });            
            return View(vehicleView);
        }

        [HttpPost]
        public IActionResult Chat(VehicleView vehicleView)
        {
            if(vehicleView.GetChatInfo.Message==null)
                return RedirectToAction("VehicleList");

            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);                        
            //vehicleView.GetChatInfo = new Chat();   
            if (User.IsInRole("Renter"))
            {
                vehicleView.GetChatInfo.SenderInfo = "Renter";
                vehicleView.GetChatInfo.RenterId = user;                
            }
            else
            {
                vehicleView.GetChatInfo.SenderInfo = "Owner";
                vehicleView.GetChatInfo.OwnerId = user;
            }            
            _context.ChatInfo.Add(vehicleView.GetChatInfo);
            _context.SaveChanges();
            return RedirectToAction("Chat", new { vehId = vehicleView.GetChatInfo.VehicleId });
           
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       

    }
}