using System.Collections.Generic;

namespace BeardBook.DAL
{
    public class FindUsersQuery : IQuery<IEnumerable<UserResult>>
    {
        public FindUsersQuery(int userId, string searchTerm, bool onlyFriends)
        {
            UserId = userId;
            SearchTerm = searchTerm;
            OnlyFriends = onlyFriends;
        }

        public int UserId { get; private set; }
        public string SearchTerm { get; private set; }
        public bool OnlyFriends { get; private set; }
    }
}
