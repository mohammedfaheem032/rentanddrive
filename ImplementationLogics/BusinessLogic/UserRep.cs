using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentNDrive.Data;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;

namespace RentNDrive.ImplementationLogics.BusinessLogic
{
    public class UserRep : IUser
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _con;
        public UserRep(ApplicationDbContext con, UserManager<UserInfo> userManager, SignInManager<UserInfo> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _con = con;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public string CreateandUpdateUser(UserInfo userDetail)
        {
            var result = _userManager.CreateAsync(userDetail, userDetail.Password);
            if (!result.Result.Succeeded)
            {
                return "Failed";
            }
            if (userDetail.OwnerName != null)
            {
                result = _userManager.AddToRoleAsync(userDetail, "Owner");
            }
            else
            {
                result = _userManager.AddToRoleAsync(userDetail, "Renter");

            }
            result.Wait();
            return "New User Successfully Created";        }

        public IEnumerable<UserInfo> GetAllUserList()
        {
            return (IEnumerable<UserInfo>)_con.Users.ToList();
        }

        public UserInfo GetUserById(string Id)
        {
            return _con.Users.FirstOrDefault(x => x.Id == Id);
        }        

        public IEnumerable<UserInfo> UserListById(string userId)
        {
            return _con.Set<UserInfo>().AsNoTracking().AsEnumerable(); 
        }
    }
}
