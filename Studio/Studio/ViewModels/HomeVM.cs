using Studio.Models;
using Studio.Services;

namespace Studio.ViewModels
{
    public class HomeVM
    {
        public List<Employee> Employees { get; set; }
        public Dictionary<string,string> Service { get; set; }
    }
}
