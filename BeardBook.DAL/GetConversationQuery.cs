using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetConversationQuery : IQuery<Conversation>
    {
        public GetConversationQuery(int userId, int friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }

        public int UserId { get; private set; }
        public int FriendId { get; private set; }
    }
}
