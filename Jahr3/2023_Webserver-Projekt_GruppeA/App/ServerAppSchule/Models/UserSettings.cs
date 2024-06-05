namespace ServerAppSchule.Models
{
    public class UserSettings
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public bool Theme { get; set; }
        public string ProfilePicture { get; set; }
        public bool FTPView { get; set; }
    }
}
