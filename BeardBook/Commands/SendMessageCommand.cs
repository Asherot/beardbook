namespace BeardBook.Commands
{
    public class SendMessageCommand
    {
        public SendMessageCommand(int userId, int conversationId, string text, string url)
        {
            UserId = userId;
            ConversationId = conversationId;
            Text = text;
            Url = url;
        }

        public int UserId { get; private set; }
        public int ConversationId { get; private set; }
        public string Text { get; private set; }
        public string Url { get; private set; }
    }
}