namespace MyWebsite.Dtos.UserDtos
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; } 
       // public string Role { get; set; } = "User"; // "Admin" veya "User"
    }
}
