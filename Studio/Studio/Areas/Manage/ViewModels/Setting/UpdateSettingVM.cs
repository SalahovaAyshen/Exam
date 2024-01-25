using System.ComponentModel.DataAnnotations;

namespace Studio.Areas.Manage.ViewModels
{
    public class UpdateSettingVM
    {
        [Required]
        public string Value { get; set; }
    }
}
