using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RentNDrive.Models.UIModel
{
    public class LoginView
    {
        [Key, StringLength(50)]
        public string UserID { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("User Name")]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }        
    }
    public class RegisterView
    {
        public UserInfo RegisterUser { get; set; }

    }
}