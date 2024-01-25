using Studio.Models;
using System.ComponentModel.DataAnnotations;

namespace Studio.Areas.Manage.ViewModels
{
    public class CreateEmployeeVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Surname { get; set; }
        public int? PositionId { get; set; }
        public List<Position>? Positions { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string TwitterLink { get; set; }
        [Required]
        public string FacebookLink { get; set; }
        [Required]
        public string LinkedinLink { get; set; }
    }
}
