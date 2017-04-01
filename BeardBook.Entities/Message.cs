using System;

namespace BeardBook.Entities
{
    public class Message
    {
        public Message()
        {
            Created = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Conversation Conversation { get; set; }
        public int ConversationId { get; set; }
    }
}
