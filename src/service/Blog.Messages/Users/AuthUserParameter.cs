#region

#endregion

using System.ComponentModel.DataAnnotations;

namespace Blog.API.Messages.Users
{
    public class AuthUserParameter : QueryParameter
    {
        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Username must be 4-30 characters")]
        public string LoginName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be 6-30 characters")]
        public string Password { get; set; }
    }
}