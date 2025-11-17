using MyWebsite.Entities;

namespace MyWebsite.Dtos.Comment
{
    public class createCommentDto
    {

        public string Content { get; set; }     
        public int UserId { get; set; }// token içinden de alınabilir şimdilik bu şekilde kalsın
        public int? ArticleId { get; set; }                
        //public int? ProjectId { get; set; }               

        //public int? ParentCommentId { get; set; }
    }
}
