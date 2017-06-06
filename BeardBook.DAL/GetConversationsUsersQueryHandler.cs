using System.Collections.Generic;
using System.Linq;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetConversationsUsersQueryHandler : IQueryHandler<GetConversationsUsersQuery, IEnumerable<User>>
    {
        private readonly BeardBookDbContext _context;

        public GetConversationsUsersQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Handle(GetConversationsUsersQuery query) =>
            _context.Users
            .First(u => u.Id == query.UserId)
            .Conversations
            .Where(c => c.Active)
            .OrderByDescending(c => c.LastUpdate)
            .SelectMany(c => c.Users)
            .Where(u => u.Id != query.UserId);
    }
}
