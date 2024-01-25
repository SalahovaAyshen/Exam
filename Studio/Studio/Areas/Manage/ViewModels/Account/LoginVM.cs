using System.ComponentModel.DataAnnotations;

namespace Studio.Areas.Manage.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
    }
}
