using System.Collections.Generic;

namespace BeardBook.Models.ConversationsViewModels
{
    public class ConversationViewModel
    {
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public string[] UserNames { get; set; }
        public string MessageText { get; set; }
        public ICollection<MessageViewModel> Messages { get; set; }
    }
}