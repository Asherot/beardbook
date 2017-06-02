using System.Linq;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetConversationQueryHandler : IQueryHandler<GetConversationQuery, Conversation>
    {
        private readonly BeardBookDbContext _context;

        public GetConversationQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public Conversation Handle(GetConversationQuery query)
        {
            var user = _context.Users.First(u => u.Id == query.UserId);

            var conversation = user.Conversations
                .Where(c => c.Active)
                .Where(c => c.Users.Count == 2)
                .FirstOrDefault(c => c.Users.Any(u => u.Id == query.FriendId))
            ?? user.Conversations
                .Where(c => c.Active)
                .OrderByDescending(c => c.LastUpdate)
                .FirstOrDefault();

            if (conversation == null) return null;

            conversation.Messages = conversation.Messages
                                                .OrderBy(m => m.Created)
                                                .ToList();

            return conversation;
        }
    }
}
