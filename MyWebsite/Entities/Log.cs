namespace MyWebsite.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Action { get; set; } = "";   // örn: "Login", "MessageSent"
        public string? IpAddress { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UserId { get; set; }
        public User? User { get; set; }

    }

}
