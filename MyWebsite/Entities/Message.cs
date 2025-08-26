namespace MyWebsite.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

}
