using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentNDrive.Data;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;
using RentNDrive.Models.UIModel;
using System.Security.Claims;

namespace RentNDrive.Controllers
{
    public class AuthenticationController : Controller
    {
        
        private readonly UserManager<UserInfo> _userManager;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IUser _userRepo;
        
        public AuthenticationController(UserManager<UserInfo> userManager,
            SignInManager<UserInfo> signInManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context, IUser userRepos)
        {            
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _userRepo = userRepos;            
        }
        // GET: AuthenticationController
        public ActionResult Login()
        {
            return View();
        }
        // POST: AuthenticationController
        [HttpPost]
        public async Task<IActionResult> Login(LoginView userInfo)
        {
            var findUser = _userManager.FindByEmailAsync(userInfo.UserEmail);
            if (findUser.Result != null)
            {
                var signIn = _signInManager.PasswordSignInAsync(findUser.Result.UserName, userInfo.Password, false, false);
                signIn.Wait();
                if (signIn.Result.Succeeded)
                {
                    //var loginstatus = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Owner") ? "Owner" : "Renter";
                    return RedirectToAction("Dashboard", "Admin");
                }
                return View(userInfo);
                // return RedirectToAction("Dashboard", "Admin");
            }            
            return View(userInfo);
        }


        // GET: AuthenticationController/Logout
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");            
        }

        // GET: AuthenticationController
        public ActionResult Register()
        {

            //ViewBag.RegisterRole = new List<SelectListItem>
            //       { new SelectListItem{Text="Owner", Value="O"},
            //         new SelectListItem{Text="Renter", Value="R"}};
            return View();
        }
        // POST: AuthenticationController
        [HttpPost]
        public async Task<IActionResult> Register(UserInfo userinfo)
        {
            if ( string.IsNullOrEmpty(userinfo.UserName))
                return View(userinfo);

         var result =   _userRepo.CreateandUpdateUser(userinfo);
            if(result != "Failed")
            return RedirectToAction("Index", "Home");

            return View(userinfo);
        }

        // POST: AuthenticationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthenticationController/Edit/5
        public ActionResult UpdateProfile()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);            
            UserInfo user = _context.Users.FirstOrDefault(x => x.Id == userId);
            return View(user);            
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserInfo userinfo)
        {

            var userReset = await _userManager.GetUserAsync(User);
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(userReset);
            var result = await _userManager.ResetPasswordAsync(userReset, resetToken, userinfo.Password);
            if (result.Succeeded)
            {
                userReset.Password = userinfo.Password; userReset.ConfirmPassword = userinfo.Password;
                userReset.PhoneNumber = userinfo.PhoneNumber;
                userReset.FirstName = userinfo.FirstName;
                userReset.LastName = userinfo.LastName;
                userReset.OwnerName = userinfo.OwnerName;
                await _userManager.UpdateAsync(userReset);

                return RedirectToAction("Index", "Home");
            }
            return View("UpdateProfile");
        }

        public IActionResult DefaultAdminandRoles()
        {
            var createRole = new RoleStore<IdentityRole>(_context);
            if (!(_context.Roles.Any()))
            {
                createRole.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                createRole.CreateAsync(new IdentityRole { Name = "Owner", NormalizedName = "OWNER" });
                createRole.CreateAsync(new IdentityRole { Name = "Renter", NormalizedName = "RENTER" });
                _context.SaveChangesAsync();
            }
            var userAdmin = new UserInfo
            {
                FirstName = "Admin",
                UserName = "Admin",
                NormalizedEmail = "admin@gmail.com",
                Email = "admin@gmail.com",
                NormalizedUserName = "Admin",
                Password = "Admin@987",
                ConfirmPassword = "Admin@987",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };



            if (!_context.Users.Any(u => u.UserName == userAdmin.UserName))
            {
                var adminuser = new UserStore<UserInfo>(_context);
                var result = _userManager.CreateAsync(userAdmin, "Admin@987");
                result.Wait();
                var roleresult = adminuser.AddToRoleAsync(userAdmin, "Admin");
                roleresult.Wait();
                TempData["Message"] = "Admin User Successfully Created";
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
