namespace ServerAppSchule.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<PostedPicture>? Pictures { get; set; }
        public List<User>? Likes { get; set; }
    }
}
