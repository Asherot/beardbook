using System;
using System.Collections.Generic;

namespace BeardBook.Entities
{
    public class Conversation
    {
        public Conversation()
        {
            LastUpdate = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
