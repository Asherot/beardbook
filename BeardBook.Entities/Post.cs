using System;
using System.Collections.Generic;

namespace BeardBook.Entities
{
    public class Post
    {
        public Post()
        {
            Created = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<File> MediaFiles { get; set; }
    }
}