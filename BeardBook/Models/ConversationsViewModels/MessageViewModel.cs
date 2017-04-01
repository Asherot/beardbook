using System;

namespace BeardBook.Models.ConversationsViewModels
{
    public class MessageViewModel
    {
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public int UserAvatarId { get; set; }
    }
}