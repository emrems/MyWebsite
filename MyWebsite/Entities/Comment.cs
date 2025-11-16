namespace MyWebsite.Entities
{
    public class Comment
    {
        public int Id { get; set; }                 
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;  
  //      public bool IsApproved { get; set; } = false; 

      
        public int UserId { get; set; }              
        public User User { get; set; } = null!;     

        public int? ArticleId { get; set; }       
        public Article? Article { get; set; }        

        //public int? ProjectId { get; set; }         
        //public Project? Project { get; set; }       

        //public int? ParentCommentId { get; set; }    
        //public Comment? ParentComment { get; set; } 
       
    }
}
