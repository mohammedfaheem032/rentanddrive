using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentNDrive.Data;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;
using RentNDrive.Models.UIModel;

namespace RentNDrive.Controllers
{
    public class OwnerController : Controller
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

        public OwnerController(UserManager<UserInfo> userManager, SignInManager<UserInfo> signInManager,
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

        // GET: Renter
        public async Task<IActionResult> Index()
        {
              return View(await _context.VehicleInfo.ToListAsync());
        }

        // GET: Renter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VehicleInfo == null)
            {
                return NotFound();
            }

            var vehicleInfo = await _context.VehicleInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleInfo == null)
            {
                return NotFound();
            }

            return View(vehicleInfo);
        }

        // GET: Renter/Create
        public IActionResult VehicleRegistration(VehicleView vehicleView, int? id)
        {
            var CurrentUserId= this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.VehicleType = _context.vehicleType.Select(x => new SelectListItem { Text = x.VehicleTypeName, Value = x.Id.ToString() });
            ViewBag.VehicleManufacturer = _context.vehicleManufacturer.Select(x => new SelectListItem { Text = x.VehicleManufacturerName, Value = x.Id.ToString() });
            vehicleView.GetAllVehicles = User.IsInRole("Owner")?  _vehicleR.GetList().Where(x => x.OwnerId == CurrentUserId).ToList():
                _vehicleR.GetList().ToList();

            if (vehicleView.GetAllVehicles.Count > 0)
            {

                foreach (var item in vehicleView.GetAllVehicles)
                {
                    item.NMVehicleManufacturer = _vehmanR.GetListById(item.VehicleManufacturer).VehicleManufacturerName; //(vehicleView.GetVehicleInfo.VehicleManufacturer).VehicleManufacturerName;
                    item.NMVehicleType = _vehtypeR.GetListById(item.VehicleType).VehicleTypeName;

                }
                return View(vehicleView);
            }
            return View();
        }

        // POST: Renter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VehicleRegistration(int?id, VehicleView vehicleInfo)
        {
            if (vehicleInfo.GetVehicleInfo==null)
            {
                return RedirectToAction("VehicleRegistration",vehicleInfo);
            }
            vehicleInfo.GetVehicleInfo.OwnerId=this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string FilePathDefault = Path.Combine(_env.WebRootPath, "VehicleImages");
            string FilePath = FilePathDefault;
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);
            var vehicleid = vehicleInfo.GetVehicleInfo.Id > 0 ? _context.VehicleInfo.Max(x=>x.Id) + 1 : 1;

            var FileName = vehicleInfo.GetVehicleInfo.NMVehicleImage.FileName;  //await _userManager.GetUserIdAsync(HttpContext.User);
            FilePath = Path.Combine(FilePathDefault, FileName);
            using (FileStream fs = System.IO.File.Create(FilePath))
            {
                vehicleInfo.GetVehicleInfo.NMVehicleImage.CopyTo(fs);
                vehicleInfo.GetVehicleInfo.VehicleImage=FileName;
            }
            
           var idval = id > 0 ? _context.Update(vehicleInfo.GetVehicleInfo) : _context.Add(vehicleInfo.GetVehicleInfo);
                await _context.SaveChangesAsync();            
            return RedirectToAction("VehicleRegistration");
        }


        // GET: Renter/Create
        public IActionResult SurveyReport()
        {
            var CurrentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            VehicleView vehicleViewModel = new VehicleView();
            vehicleViewModel.GetAllSurvey = _vehicleR.GetSurveyList().Where(u => u.OwnerId == CurrentUserId).ToList();
            vehicleViewModel.GetAllSurvey.ForEach(c =>
            {
                c.NMOwnerName = _userR.GetUserById(CurrentUserId).OwnerName;
                c.NMRenterName = _userR.GetUserById(c.RenterId).FirstName;
            });
            vehicleViewModel.GetAllVehicles = User.IsInRole("Owner") ? _vehicleR.GetList().Where(x => x.OwnerId == CurrentUserId).ToList() :
                _vehicleR.GetList().ToList();
            if (vehicleViewModel.GetAllVehicles.Count > 0)
            {
                foreach (var item in vehicleViewModel.GetAllVehicles)
                {
                    item.NMVehicleManufacturer = _vehmanR.GetListById(item.VehicleManufacturer).VehicleManufacturerName; //(vehicleView.GetVehicleInfo.VehicleManufacturer).VehicleManufacturerName;
                    item.NMVehicleType = _vehtypeR.GetListById(item.VehicleType).VehicleTypeName;
                }
                return View(vehicleViewModel);
            }
            return View();
        }



        // GET: Renter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VehicleInfo == null)
            {
                return NotFound();
            }

            var vehicleInfo = await _context.VehicleInfo.FindAsync(id);
            if (vehicleInfo == null)
            {
                return NotFound();
            }
            return View(vehicleInfo);
        }

        // POST: Renter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,VehicleName,VehicleNo,RegistrationDate,VehicleType,VehicleManufacturer,VehiclePrice,PriceType,Doors,Passengers,Luggage,Transmission,AirConditioning,Terms,VehicleAvailableStatus,FeatureVehicle,VehicleImage,VehicleActiveStatus")] VehicleInfo vehicleInfo)
        {
            if (id != vehicleInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleInfoExists(vehicleInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleInfo);
        }

        // GET: Renter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VehicleInfo == null)
            {
                return NotFound();
            }

            var vehicleInfo = await _context.VehicleInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleInfo == null)
            {
                return NotFound();
            }

            return View(vehicleInfo);
        }

        // POST: Renter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VehicleInfo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.VehicleInfo'  is null.");
            }
            var vehicleInfo = await _context.VehicleInfo.FindAsync(id);
            if (vehicleInfo != null)
            {
                _context.VehicleInfo.Remove(vehicleInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleInfoExists(int id)
        {
          return _context.VehicleInfo.Any(e => e.Id == id);
        }
    }
}
