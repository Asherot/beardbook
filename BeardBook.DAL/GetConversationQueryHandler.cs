using System.Linq;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetConversationQueryHandler :IQueryHandler<GetConversationQuery, Conversation>
    {
        private readonly BeardBookDbContext _context;

        public GetConversationQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public Conversation Handle(GetConversationQuery query)
        {
            var conversation = query.FriendId != 0
                            ? _context.Conversations
                                    .FirstOrDefault(c => c.Users.Count == 2
                                                        && c.Users.Any(u => u.Id == query.UserId)
                                                        && c.Users.Any(u => u.Id == query.FriendId)
                                                        && c.Active) 
                            : _context.Conversations
                                   .Where(c => c.Active
                                            && c.Users.Any(u => u.Id == query.UserId))
                                   .OrderByDescending(c => c.LastUpdate)
                                   .FirstOrDefault();

            if (conversation == null) return null;

            conversation.Messages = conversation.Messages.OrderBy(m => m.Created).ToList();

            return conversation;
        }
    }
}
