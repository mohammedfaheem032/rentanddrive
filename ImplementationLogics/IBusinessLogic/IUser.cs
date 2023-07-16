using RentNDrive.Models;

namespace RentNDrive.ImplementationLogics.IBusinessLogic
{
    public interface IUser
    {
        string CreateandUpdateUser(UserInfo userDetail);        
        UserInfo GetUserById(string Id);
        IEnumerable<UserInfo> UserListById(string userId);
        IEnumerable<UserInfo> GetAllUserList();
    }
}
