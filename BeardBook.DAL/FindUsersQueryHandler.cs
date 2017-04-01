using System.Collections.Generic;
using System.Linq;
using BeardBook.Entities;

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
            var user = _context.Users.First(u => u.Id == query.UserId);
            var users = _context.Users
                .Where(u =>
                    u.Id != query.UserId
                    && (query.SearchTerm == null
                    || u.FirstName.StartsWith(query.SearchTerm)
                    || u.LastName.StartsWith(query.SearchTerm))).ToList();
            var userResults = users
                .Select(u => new UserResult
                {
                    User = u,
                    IsFriend = user.Friends.Any(f => f.Id == u.Id)

                });
            return query.OnlyFriends 
                ? userResults.Where(u => u.IsFriend).ToList() 
                : userResults.ToList();
        }
    }

    public class UserResult
    {
        public User User { get; set; }
        public bool IsFriend { get; set; }
    }
}
