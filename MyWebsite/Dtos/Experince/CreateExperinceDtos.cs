namespace MyWebsite.Dtos.Experince
{
    public class CreateExperinceDtos
    {
        public string Company { get; set; }
        public string Role { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
    }
}
