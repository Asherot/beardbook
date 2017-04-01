using System;

namespace BeardBook.Entities
{
    public class File
    {
        public File()
        {
            Created = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public FileType FileType { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public DateTime Created { get; set; }
        public virtual Post Post { get; set; }
        public int? PostId { get; set; }

        public string ToBase64()
            => $"data:{ContentType};base64,{Convert.ToBase64String(Data)}";
    }

    public enum FileType
    {
        Photo,
        Avatar,
        Video,
        Other
    }
}