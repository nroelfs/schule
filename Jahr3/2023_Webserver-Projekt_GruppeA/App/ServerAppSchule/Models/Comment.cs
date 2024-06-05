namespace ServerAppSchule.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
}
