using System.ComponentModel.DataAnnotations;

namespace QOC.Application.DTOs.User
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FullName is required")]
        [MinLength(3, ErrorMessage = "FullName must be at least 3 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        public string OldPassword { get; set; }

        [MinLength(6, ErrorMessage = "New password must be at least 6 characters")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "New password and confirmation must match")]
        public string ConfirmNewPassword { get; set; }
    }
}
