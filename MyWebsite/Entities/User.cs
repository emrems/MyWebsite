namespace MyWebsite.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "User"; // "Admin" veya "User"
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Article>? Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
