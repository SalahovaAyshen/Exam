namespace Studio.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public string TwitterLink { get; set; }
        public string FacebookLink { get; set; }
        public string LinkedinLink { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
