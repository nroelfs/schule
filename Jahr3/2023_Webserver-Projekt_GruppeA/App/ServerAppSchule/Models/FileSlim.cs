namespace ServerAppSchule.Models
{
    public class FileSlim
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double? Size { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
