namespace ServerAppSchule.Models
{
    public class PostedPicture
    {
        public int Id { get; set; }
        public string PictureAsBase64 { get; set; }
        public double Size { get; set; }
        public string Type { get; set; }
        public int PostId { get; set; }
    }
}
