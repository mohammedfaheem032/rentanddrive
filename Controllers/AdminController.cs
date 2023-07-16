using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentNDrive.Data;
using RentNDrive.ImplementationLogics.BusinessLogic;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;
using RentNDrive.Models.UIModel;
using System.Linq;
using System.Security.Claims;

namespace RentNDrive.Controllers
{
    public class AdminController : Controller
    {
        
        private readonly UserManager<UserInfo> _userManager;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IVehType _vehtypeR;
        private readonly IVehManufacturer _vehmanR;
        private readonly IUser _userR;
        private readonly IVehicle _vehicleR;

        public AdminController(UserManager<UserInfo> userManager, SignInManager<UserInfo> signInManager,
                RoleManager<IdentityRole> roleManager, IVehicle vehicleR, IUser userR, ApplicationDbContext context,IVehType vehtypeR, IVehManufacturer vehmanR)
        {
            _userManager = userManager;   
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _vehtypeR = vehtypeR;
            _vehmanR=vehmanR;
            _userR = userR;
            _vehicleR = vehicleR;
        }
        // GET: AdminController
        public ActionResult Dashboard()
        {
            var currentuserid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.VehType = _vehtypeR.GetList().Count();
            ViewBag.VehMan = _vehmanR.GetList().Count();
            ViewBag.VehList = User.IsInRole("Admin") ? _vehicleR.GetList().Count() : _vehicleR.GetList().Where(x=>x.OwnerId == currentuserid).Count();
            if (User.IsInRole("Owner"))
            {
                var vehId = _vehicleR.GetList().Where(x => x.OwnerId == currentuserid).ToList();
                foreach (var item in vehId)
                {
                    ViewBag.BookList = _vehicleR.GetBookList().Where(x => x.VehicleId == item.Id).Count();
                }
            }
            ViewBag.BookList = User.IsInRole("Renter") ? _vehicleR.GetBookList().Where(x => x.RenterId == currentuserid).Count() : _vehicleR.GetBookList().Count();
            ViewBag.ChatList = User.IsInRole("Renter") ? _vehicleR.GetChatList().Where(x => x.RenterId == currentuserid).Count() : User.IsInRole("Owner") ? _vehicleR.GetChatList().Where(x => x.OwnerId == currentuserid).Count() :_vehicleR.GetChatList().Count();
            ViewBag.OwnList = _userR.GetAllUserList().Where(o=>o.OwnerName!=null).Count();
            ViewBag.RentList = _userR.GetAllUserList().Where(o => o.OwnerName == null && o.UserName!="Admin").Count();
            return View();
        }
        // GET: AdminController
        [HttpGet]
        public IActionResult carType(int? id)
        {
           var vehType =new VehTypeView();
            vehType.vehicleTypeList = _vehtypeR.GetList(); 
            var idValue = id > 0 ? vehType.vehicleTypeNew = _vehtypeR.GetListById((int)id) : vehType.vehicleTypeNew = new VehicleType();
            return View(vehType);
        }
        // POST: AdminController
        [HttpPost]
        public IActionResult carType(int id, VehTypeView vehType)
        {
            if (vehType.vehicleTypeNew == null)
            {
                return View(vehType); 
            }
            var result= _vehtypeR.CreateandUpdateVType(id,vehType.vehicleTypeNew);
            if (result == "Success")
            { return RedirectToAction("carType"); }
            return View(vehType);
        }
        public ActionResult Cancel()
        {
            var vehicleTypeClear = new VehicleType(); 
            return RedirectToAction("carType");
        }

        // GET: AdminController/Create
        [HttpGet]
        public IActionResult VehicleManufacturer(int? id)
        {
            var vehMan = new VehManufacturerView();
            vehMan.vehicleManList = _vehmanR.GetList();
            var idValue = id > 0 ? vehMan.vehicleManNew = _vehmanR.GetListById((int)id) : vehMan.vehicleManNew = new VehicleManufacturer();
            return View(vehMan);
        }
        // POST: AdminController
        [HttpPost]
        public IActionResult VehicleManufacturer(VehManufacturerView vehManview)
        {
            if (vehManview.vehicleManNew == null)
            {
                return View(vehManview);
            }
            var result = _vehmanR.CreateandUpdateVManufacturer(vehManview.vehicleManNew.Id, vehManview.vehicleManNew);
            if (result == "Success")
                return RedirectToAction("VehicleManufacturer");
            //{ vehManview.vehicleManList = _vehmanR.GetList(); vehManview.vehicleManNew = new VehicleManufacturer();}
            return View(vehManview);
        }
        public ActionResult ManufactCancel()
        {
            var vehicleManClear = new VehicleManufacturer();
            return RedirectToAction("VehicleManufacturer");
        }

        [HttpGet]
        public async Task<IActionResult> OwnerList(AdminView adminView, string uservalue)
        {
            ViewBag.uservalue = uservalue;
            adminView.UserList = uservalue == "Owner"? _userR.GetAllUserList().Where(o=>o.OwnerName != null) :  _userR.GetAllUserList().Where(o => o.OwnerName == null && o.UserName!="Admin");
            return View("UsersList",adminView);
        }        
                     
        public IActionResult ActiveInactive(string? id, int? vehicleId)
        {
            var status = _userR.GetUserById(id);//_context.Users.Where(x => x.Id == id).ToList().FirstOrDefault();//_userR.UserListById(id);            
            if (status != null)
            {
                status.EmailConfirmed = !status.EmailConfirmed;
                _context.Entry(status).State = EntityState.Modified;
                //_context.Users.Update(status); 
                _context.SaveChanges();
                var userval = status.OwnerName != null ? "Owner" : "Renter";
                return RedirectToAction("OwnerList", new { uservalue = userval });
            }
            var vehiclestatus = _vehicleR.GetListById((int)vehicleId);
            if (vehiclestatus != null)
            {
                vehiclestatus.VehicleActiveStatus = !vehiclestatus.VehicleActiveStatus;
                _context.Entry(vehiclestatus).State = EntityState.Modified;                
                _context.SaveChanges();                
                return RedirectToAction("VehicleRegistration","Owner");
            }
            return RedirectToAction("Dashboard");
        }
    }
}
