using NaradX.Entities.Response.Auth;
using System.ComponentModel.DataAnnotations;

namespace NaradX.Web.ViewModels.Auth
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter Email ID")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email Id is not valid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
        public string? IpAddress { get; set; }
        public string? ReturnUrl { get; set; }
        public LoginResponse? LoginResponse { get; set; }
    }
}
