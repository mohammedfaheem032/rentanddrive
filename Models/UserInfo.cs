using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentNDrive.Models
{
    public class UserInfo : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Owner Name")]
        public string? OwnerName { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password are incompatible")]
        [DisplayName("RePassword")]
        public string ConfirmPassword { get; set; }
    }
}
