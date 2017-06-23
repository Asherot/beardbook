using System.Collections.Generic;
using System.Linq;

namespace BeardBook.DAL
{
    public class FindUsersQueryHandler : IQueryHandler<FindUsersQuery, IEnumerable<UserResult>>
    {
        private readonly BeardBookDbContext _context;

        public FindUsersQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserResult> Handle(FindUsersQuery query)
        {
            var userResults = _context.Users
                .Where(u => u.Id != query.UserId)
                .Where(u => query.SearchTerm == null
                         || u.FirstName.StartsWith(query.SearchTerm)
                         || u.LastName.StartsWith(query.SearchTerm))
                .ToList()
                .Select(u => new UserResult
                    (u, u.Friends.Any(f => f.Id == query.UserId)));

            return query.OnlyFriends 
                ? userResults.Where(u => u.IsFriend) 
                : userResults;
        }
    }
}
